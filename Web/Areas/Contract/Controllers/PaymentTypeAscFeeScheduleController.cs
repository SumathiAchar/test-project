using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypeAscFeeScheduleController : CommonController
    {

        /// <summary>
        /// Payments the type asc fee schedule details.
        /// </summary>
        /// <param name="contractId">The contract unique identifier.</param>
        /// <param name="serviceTypeId">The service type unique identifier.</param>
        /// <param name="paymentTypeId">The payment type unique identifier.</param>
        /// <param name="isEdit">if set to <c>true</c> [is edit].</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PaymentTypeAscFeeSchedule(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypeAscFeeScheduleViewModel paymentTypeAscFeeScheduleViewModel = new PaymentTypeAscFeeScheduleViewModel();

            List<AscFeeScheduleOption> ascFeeScheduleOptions =
                GetApiResponse<List<AscFeeScheduleOption>>(Constants.PaymentTypeAscFeeSchedule, Constants.GetAscFeeScheduleOptions);

            List<AscFeeScheduleOptionViewModel> ascFeeScheduleOptionsData =
               AutoMapper.Mapper.Map<List<AscFeeScheduleOption>, List<AscFeeScheduleOptionViewModel>>(ascFeeScheduleOptions);

            if (isEdit)
            {
                PaymentTypeAscFeeSchedule paymentTypeAscFeeScheduleDetails = new PaymentTypeAscFeeSchedule
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };

                PaymentTypeAscFeeSchedule paymentTypeAscFeeScheduleViewModelInfo =
                    PostApiResponse<PaymentTypeAscFeeSchedule>(Constants.PaymentTypeAscFeeSchedule,
                        "GetPaymentTypeAscFeeSchedule",
                        paymentTypeAscFeeScheduleDetails);

                paymentTypeAscFeeScheduleViewModel = AutoMapper.Mapper.Map<PaymentTypeAscFeeSchedule, PaymentTypeAscFeeScheduleViewModel>(paymentTypeAscFeeScheduleViewModelInfo);
            }

            paymentTypeAscFeeScheduleViewModel.ContractId = contractId;
            paymentTypeAscFeeScheduleViewModel.ServiceTypeId = serviceTypeId;
            paymentTypeAscFeeScheduleViewModel.PaymentTypeId = paymentTypeId;
            paymentTypeAscFeeScheduleViewModel.IsEdit = isEdit;
            paymentTypeAscFeeScheduleViewModel.AscFeeScheduleOption = ascFeeScheduleOptionsData;
               return View(paymentTypeAscFeeScheduleViewModel);
        }

        /// <summary>
        /// Adds the payment type asc fee.
        /// </summary>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult AddEditPaymentTypeAscFee(PaymentTypeAscFeeScheduleViewModel ascFeeScheduleInfo)
        {
            long ascFeeScheduleId = 0;
            if (ascFeeScheduleInfo.ClaimFieldDocId != null)
            {
                PaymentTypeAscFeeSchedule ascFeeSchedule =
                    AutoMapper.Mapper.Map<PaymentTypeAscFeeScheduleViewModel, PaymentTypeAscFeeSchedule>(
                        ascFeeScheduleInfo);
                //Get the Name of User logged in
                ascFeeSchedule.UserName = GetCurrentUserName();
                ascFeeScheduleId = PostApiResponse<long>(Constants.PaymentTypeAscFeeSchedule,
                    Constants.AddEditPaymentTypeAscFeeSchedule, ascFeeSchedule);
            }
            return ascFeeScheduleId > 0 ? Json(new { sucess = true, documentId = ascFeeScheduleInfo.ClaimFieldDocId }) : Json(new { sucess = false, documentId = ascFeeScheduleInfo.ClaimFieldDocId });
        }
        
        ///// <summary>
        ///// Get TableName Selection basing on claimFieldId & contractId
        ///// </summary>
        [HttpPost]
        public JsonResult GetTableNameSelection(long paymentTypeId, long? nodeId)
        {
            PaymentTypeAscFeeSchedule paymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule();

            switch ((Enums.PaymentTypeCodes)paymentTypeId)
            {
                case Enums.PaymentTypeCodes.AscFeeSchedule:
                    paymentTypeAscFeeSchedule.ClaimFieldId = EnumHelperLibrary.GetFieldInfoFromEnum(Enums.ClaimFieldTypes.AscFeeSchedule).FieldIdentityNumber;
                    break;
                case Enums.PaymentTypeCodes.DrgPayment:
                    paymentTypeAscFeeSchedule.ClaimFieldId = EnumHelperLibrary.GetFieldInfoFromEnum(Enums.ClaimFieldTypes.DrgWeightTable).FieldIdentityNumber;
                    break;
                case Enums.PaymentTypeCodes.FeeSchedule:
                    paymentTypeAscFeeSchedule.ClaimFieldId = EnumHelperLibrary.GetFieldInfoFromEnum(Enums.ClaimFieldTypes.FeeSchedule).FieldIdentityNumber;
                    break;
                case Enums.PaymentTypeCodes.CustomTableFormulas:
                    paymentTypeAscFeeSchedule.ClaimFieldId = EnumHelperLibrary.GetFieldInfoFromEnum(Enums.ClaimFieldTypes.CustomPaymentType).FieldIdentityNumber;
                    break;
            }
            paymentTypeAscFeeSchedule.NodeId = nodeId;

            //Get the Name of User logged in
            paymentTypeAscFeeSchedule.UserName = GetCurrentUserName();
            List<PaymentTypeTableSelection> paymentTypeTableSelection = PostApiResponse<List<PaymentTypeTableSelection>>(Constants.PaymentTypeAscFeeSchedule, Constants.GetTableNameSelection, paymentTypeAscFeeSchedule);
            List<SelectListItem> paymentTableSelection = new List<SelectListItem>();
            if (paymentTypeTableSelection != null && paymentTypeTableSelection.Count > 0)
            {
                paymentTableSelection.AddRange(paymentTypeTableSelection.Select(item => new SelectListItem { Text = item.TableName, Value = Convert.ToString(item.ClaimFieldDocId) }));  
            }
            return Json(new { tableSelectionList = paymentTableSelection});
        }
    }
}
