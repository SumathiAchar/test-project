using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ContractServiceLineClaimFieldSelectionController : CommonController
    {
        public ActionResult ContractServiceLineClaimFieldSelection(long? contractId, long? serviceTypeId, long serviceLineTypeId, bool isEdit)
        {
            ContractServiceLineClaimFieldSelectionViewModel modelContractServiceLineClaimFieldSelection = new ContractServiceLineClaimFieldSelectionViewModel();
            if (isEdit)
            {
                ContractServiceLineClaimFieldSelection contractServiceLineClaimFieldSelectionforPost =
                    new ContractServiceLineClaimFieldSelection
                    {
                        ContractId = contractId,
                        ServiceLineTypeId = serviceLineTypeId,
                        ContractServiceTypeId = serviceTypeId,
                        UserName = GetCurrentUserName()
                    };

                //Get the Name of User logged in
                List<ContractServiceLineClaimFieldSelection> contractServiceLineClaimFieldSelectionViewModelInfo = PostApiResponse<List<ContractServiceLineClaimFieldSelection>>("ContractServiceLineClaimFieldSelection",
                                                           "GetClaimFieldSelection",
                                                           contractServiceLineClaimFieldSelectionforPost);
                List<ContractServiceLineClaimFieldSelectionViewModel> serviceLineClaimFieldSelection = AutoMapper.Mapper.Map<List<ContractServiceLineClaimFieldSelection>, List<ContractServiceLineClaimFieldSelectionViewModel>>(contractServiceLineClaimFieldSelectionViewModelInfo);
                modelContractServiceLineClaimFieldSelection.ContractServiceLineClaimFieldSelectionList = serviceLineClaimFieldSelection;
            }
            modelContractServiceLineClaimFieldSelection.ContractId = contractId;
            modelContractServiceLineClaimFieldSelection.ContractServiceTypeId = serviceTypeId;
            modelContractServiceLineClaimFieldSelection.ServiceLineTypeId = serviceLineTypeId;
            modelContractServiceLineClaimFieldSelection.IsEdit = isEdit;
            modelContractServiceLineClaimFieldSelection.ModuleId = Convert.ToByte(EnumHelperLibrary.GetFieldInfoFromEnum(Enums.Modules.ClaimToolClaimField).FieldIdentityNumber);
            return View(modelContractServiceLineClaimFieldSelection);
        }

        /// <summary>
        /// Edit the Claim Field selection method
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        [HttpPost]
        public JsonResult AddEditClaimFieldSelection(List<ContractServiceLineClaimFieldSelectionViewModel> listofserviceLineClaims, bool isEdited)
        {
            List<ContractServiceLineClaimFieldSelection> modelClaimFeild = new List<ContractServiceLineClaimFieldSelection>();
            if (listofserviceLineClaims != null && listofserviceLineClaims.Count > 0)
            {
                modelClaimFeild.AddRange(listofserviceLineClaims.Select(AutoMapper.Mapper.Map<ContractServiceLineClaimFieldSelectionViewModel, ContractServiceLineClaimFieldSelection>));
            }
            //Get the Name of User logged in
            foreach (var contractServiceLineClaimFieldSelection in modelClaimFeild)
            {
                contractServiceLineClaimFieldSelection.UserName = GetCurrentUserName();
            }
            long id = isEdited
                ? PostApiResponse<long>("ContractServiceLineClaimFieldSelection", "EditClaimFieldSelection",
                    modelClaimFeild)
                : PostApiResponse<long>("ContractServiceLineClaimFieldSelection", "AddNewClaimFieldSelection",
                    modelClaimFeild);
            bool isSuccess = id > 0;
            return Json(new { sucess = isSuccess, addedId = id });
        }

        /// <summary>
        /// Get the all Claim Fields Operator 
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public JsonResult GetClaimFieldOperator()
        {
            List<ClaimFieldOperator> contractClaimFeildsOperator = GetApiResponse<List<ClaimFieldOperator>>("ReportSelection", "GetAllClaimFieldsOperators");
            List<SelectListItem> contractServiceClaims = new List<SelectListItem>();
            if (contractClaimFeildsOperator != null && contractClaimFeildsOperator.Count > 0)
            {
                contractServiceClaims.AddRange(
                    contractClaimFeildsOperator.Select(
                        item =>
                            new SelectListItem
                            {
                                Text = item.OperatorType,
                                Value = item.OperatorId.ToString(CultureInfo.InvariantCulture)
                            }));
            }
            return Json(new { claimFeildOperatorList = contractServiceClaims }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the claim reviewed option.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetClaimReviewedOption()
        {
            List<ReviewedOptionType> reviewdOptions = GetApiResponse<List<ReviewedOptionType>>(Constants.ReportSelection, Constants.GetClaimReviewedOption);
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            if (reviewdOptions != null && reviewdOptions.Count > 0)
            {
                selectListItems.AddRange(
                    reviewdOptions.Select(
                        item =>
                            new SelectListItem
                            {
                                Text = item.ReviewedOption,
                                Value = item.ReviewedOptionId.ToString(CultureInfo.InvariantCulture)
                            }));
            }
            return Json(new { reviewedOptionLists = selectListItems }, JsonRequestBehavior.AllowGet);
        }
    }
}