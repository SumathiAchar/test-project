using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ContractInfoController : CommonController
    {
        //
        // GET: /ContractInfo/
        public ActionResult ContractInfo()
        {
            return View();
        }

        /// <summary>
        /// Add contract
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddContractInfo(ContractInfoViewModel info)
        {
            ContractPayerInfo payermodel = AutoMapper.Mapper.Map<ContractInfoViewModel, ContractPayerInfo>(info);
            //Get the Name of User logged in
            payermodel.UserName = GetCurrentUserName();
            long id = PostApiResponse<long>("ContractPayerInfo", "AddEditContractPayerInfo", payermodel);
            return Json(new { addedId = id });
        }

        /// <summary>
        /// Get Contract
        /// </summary>
        /// <param name="conrtactPayerInfoId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetContractInfo(long conrtactPayerInfoId)
        {
            ContractPayerInfo contractPayerInfo = GetApiResponse<ContractPayerInfo>("ContractPayerInfo", "GetContractPayerInfo", conrtactPayerInfoId);
            return Json(new { data = contractPayerInfo });
        }

        /// <summary>
        /// Delete Contract
        /// </summary>
        /// <param name="conrtactPayerInfoId"></param>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteContractInfo(long conrtactPayerInfoId, long contractId)
        {
            ContractPayerInfo contractPayerInfo = new ContractPayerInfo
            {
                UserName = GetCurrentUserName(),
                ContractPayerInfoId = conrtactPayerInfoId,
                ContractId = contractId
            };
            //Get the UserName logged in
            bool isSuccess = PostApiResponse<bool>("ContractPayerInfo", "DeleteContractPayerInfoById", contractPayerInfo);
            return Json(new { sucess = isSuccess });
        }
    }
}
