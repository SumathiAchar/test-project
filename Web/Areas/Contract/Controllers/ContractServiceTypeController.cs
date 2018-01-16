using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ContractServiceTypeController : SessionStoreController
    {
        /// <summary>
        /// This action method will be called once you click on 'Add Service Type' in Context menu in tree
        /// </summary>
        /// <param name="contractId">contractId where user wants to add new service type</param>
        /// <param name="nodeId">The node unique identifier.</param>
        /// <param name="serviceTypeId"></param>
        /// <param name="isEdit"></param>
        /// <returns>
        /// View
        /// </returns>
        public ActionResult ContractServiceType(long contractId, long nodeId, long serviceTypeId, bool isEdit)
        {
            ContractServiceTypeViewModel model = new ContractServiceTypeViewModel();
            if (isEdit)
            {
                ContractServiceType contractServiceType = new ContractServiceType
                {
                    ContractServiceTypeId =
                        serviceTypeId,
                    ContractId = contractId,
                    UserName = GetLoggedInUserName()
                };

                //Get the Name of User logged in
                ContractServiceType contractServiceTypeInfo =
             PostApiResponse<ContractServiceType>("ContractServiceType",
                                                           "GetContractServiceTypeDetails",
                                                           contractServiceType);

                model = AutoMapper.Mapper.Map<ContractServiceType, ContractServiceTypeViewModel>(contractServiceTypeInfo);
            }
            model.ContractId = contractId;
            model.ContractNodeId = nodeId;
            model.IsEdit = isEdit;
            return View(model);
        }

        /// <summary>
        /// Returns all available service type 
        /// </summary>
        /// <returns>Json object that contains List of SelectListItem</returns>
        public JsonResult GetAllServiceType(long contractId)
        {
            List<ContractServiceType> contractServiceTypes = GetApiResponse<List<ContractServiceType>>("ContractServiceType", "GetAllContractServiceType", contractId);
            List<SelectListItem> contractServiceTypeList = new List<SelectListItem>();
            if (contractServiceTypes != null && contractServiceTypes.Count > 0)
            {
                contractServiceTypeList.AddRange(
                    contractServiceTypes.Select(
                        item =>
                            new SelectListItem
                            {
                                Text = item.ContractServiceTypeName,
                                Value = item.ContractServiceTypeId.ToString(CultureInfo.InvariantCulture)
                            }));
            }
            return Json(new { serviceTypeList = contractServiceTypeList });
        }

        /// <summary>
        /// Add ContractServiceType based on passed ServiceTypeViewModel object 
        /// </summary>
        /// <param name="serviceTypeViewModel">Object of ServiceTypeViewModel</param>
        /// <returns>returns Json object with newly inserted service type id</returns>
        [HttpPost]
        public JsonResult AddEditServiceDetails(ContractServiceTypeViewModel serviceTypeViewModel)
        {
            ContractServiceType contractServiceType = AutoMapper.Mapper.Map<ContractServiceTypeViewModel, ContractServiceType>(serviceTypeViewModel);
            //Get the Name of User logged in
            contractServiceType.UserName = GetLoggedInUserName();
            long contractServiceTypeId = PostApiResponse<long>("ContractServiceType", "AddEditContractServiceType", contractServiceType);
            LastExpandedNodeId = serviceTypeViewModel.ContractNodeId;
            LastHighlightedNodeId = contractServiceTypeId;
            return Json(new { insertedContractServiceTypeId = contractServiceTypeId });
        }

        /// <summary>
        /// Checks the contract service type name is unique.
        /// </summary>
        /// <param name="serviceTypeViewModel">The service type view model.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsContractServiceTypeNameExit(ContractServiceTypeViewModel serviceTypeViewModel)
        {
            ContractServiceType contractServiceType = AutoMapper.Mapper.Map<ContractServiceTypeViewModel, ContractServiceType>(serviceTypeViewModel);
            //Get the Name of User logged in
            contractServiceType.UserName = GetLoggedInUserName();
            return Json(PostApiResponse<bool>(Shared.Helpers.Constants.ContractServiceType, Shared.Helpers.Constants.IsContractServiceTypeNameExit, contractServiceType));
        }
    }
}
