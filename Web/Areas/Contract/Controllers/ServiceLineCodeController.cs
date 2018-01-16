using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ServiceLineCodeController : CommonController
    {
        //
        // GET: /ServiceLineCode/

        public ActionResult ServiceLineCode(long? contractId, long? serviceTypeId, long serviceLineTypeId, bool isEdit)
        {
            ContractServiceLine inputmodel = new ContractServiceLine
            {
                ServiceLineId = serviceLineTypeId,
                PageSize = Convert.ToInt16(GlobalConfigVariable.ServiceLineCodesPageSize),
                PageIndex = 1 // Sets to first page
            };
            List<ServiceLineCode> codecount = PostApiResponse<List<ServiceLineCode>>("ServiceLineCode", "GetAllServiceLineCodes", inputmodel);
            ContractServiceLineViewModel model = new ContractServiceLineViewModel();
            if (isEdit)
            {
                ContractServiceLine contractServiceLineforPost = new ContractServiceLine
                {
                    ContractId = contractId,
                    ServiceLineId = serviceLineTypeId,
                    ContractServiceTypeId = serviceTypeId,
                    PageSize = Convert.ToInt16(GlobalConfigVariable.ServiceLineCodesPageSize),
                    PageIndex = 1, // Sets to first page
                    TotalRecords = (codecount != null && codecount.Count > 0) ? codecount[0].TotalRecs : 0,
                    UserName = GetCurrentUserName()
                };
                //Get the Name of User logged in
                ContractServiceLine contractServiceLineViewModelInfo = PostApiResponse<ContractServiceLine>("ServiceLineCode",
                                                           "GetServiceLineCodeDetails",
                                                           contractServiceLineforPost);

                model = AutoMapper.Mapper.Map<ContractServiceLine, ContractServiceLineViewModel>(contractServiceLineViewModelInfo);

            }
            model.ContractId = contractId;
            model.ContractServiceTypeId = serviceTypeId;
            model.ServiceLineTypeId = serviceLineTypeId;
            model.IsEdit = isEdit;
            model.PageIndex = 1;// Sets to first page
            model.PageSize = Convert.ToInt16(GlobalConfigVariable.ServiceLineCodesPageSize);
            model.TotalRecords = (codecount != null && codecount.Count > 0) ? codecount[0].TotalRecs : 0;
            return View(model);
        }

        /// <summary>
        /// Add ServiceLine Code Details 
        /// </summary>
        /// <param name="serviceLineinfo">ContractServiceLineViewModel object</param>
        /// <returns>Json Object</returns>
        [HttpPost]
        public JsonResult AddEditServiceLineCode(ContractServiceLineViewModel serviceLineinfo)
        {
            ContractServiceLine modelContractServiceLine = AutoMapper.Mapper.Map<ContractServiceLineViewModel, ContractServiceLine>(serviceLineinfo);
            //Get the Name of User logged in
            modelContractServiceLine.UserName = GetCurrentUserName();
            long noOfRecord = PostApiResponse<long>("ServiceLineCode", "AddEditServiceLineCodeDetails", modelContractServiceLine);
            return noOfRecord > 0 ? Json(new { sucess = true, Id = noOfRecord }) : Json(new { sucess = false });
        }

        /// <summary>
        /// Get the all ServiceLine RevenueCodes basing on serviceTypeId
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        //Storing ServiceLines data into Cache for one day
        public ContentResult GetServiceLineCodesSelection(long serviceLineTypeId, int pageSize, int pageIndex)
        {
            ContractServiceLine inputmodel = new ContractServiceLine
            {
                ServiceLineId = serviceLineTypeId,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            List<ServiceLineCode> serviceLineCode = PostApiResponse<List<ServiceLineCode>>("ServiceLineCode", "GetAllServiceLineCodes", inputmodel);

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var resultData = new { slCodes = serviceLineCode };
            //serializing data
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;
        }
    }
}
