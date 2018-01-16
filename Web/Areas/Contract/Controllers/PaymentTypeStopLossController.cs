using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypeStopLossController : CommonController
    {
        //
        // GET: /PaymentTypeStopLoss/
        public ActionResult PaymentTypeStopLoss(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypeStopLossViewModel modelPaymentTypeStopLossViewModel = new PaymentTypeStopLossViewModel();
            if (isEdit)
            {
                PaymentTypeStopLoss paymentTypeStopLossForPost = new PaymentTypeStopLoss
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };

                //Get the Name of User logged in
                PaymentTypeStopLoss paymentTypeStopLossViewModelInfo =
             PostApiResponse<PaymentTypeStopLoss>("PaymentTypeStopLoss",
                                                           "GetPaymentTypeStopLoss",
                                                           paymentTypeStopLossForPost);

                modelPaymentTypeStopLossViewModel = AutoMapper.Mapper.Map<PaymentTypeStopLoss, PaymentTypeStopLossViewModel>(paymentTypeStopLossViewModelInfo);
            }

            modelPaymentTypeStopLossViewModel.ContractId = contractId;
            modelPaymentTypeStopLossViewModel.ServiceTypeId = serviceTypeId;
            modelPaymentTypeStopLossViewModel.PaymentTypeId = paymentTypeId;
            modelPaymentTypeStopLossViewModel.IsEdit = isEdit;
            modelPaymentTypeStopLossViewModel.StopLossConditions = GetApiResponse<List<StopLossCondition>>("PaymentTypeStopLoss",
                                                           "GetPaymentTypeStopLossConditions");
            return View(modelPaymentTypeStopLossViewModel);
        }

        /// <summary>
        /// Adds the edit payment type stop loss.
        /// </summary>
        /// <param name="paymentTypeStopLossViewModel">The information.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditPaymentTypeStopLoss(PaymentTypeStopLossViewModel paymentTypeStopLossViewModel)
        {
            PaymentTypeStopLoss stopLossPaymentInfo =
                        AutoMapper.Mapper.Map<PaymentTypeStopLossViewModel, PaymentTypeStopLoss>(paymentTypeStopLossViewModel);
            long stoplossId = 0;
            bool isValidated = Utilities.ValidateExpression(stopLossPaymentInfo.Expression, Constants.StopLossClaimValuePair);
            if (isValidated)
            {
                stopLossPaymentInfo.UserName = GetCurrentUserName();
                stoplossId = PostApiResponse<long>(Constants.PaymentTypeStopLoss, Convert.ToString(Enums.Action.AddEditPaymentTypeStopLoss), stopLossPaymentInfo);
            }
            return Json(new { success = isValidated, Id = stoplossId });
        }
    }
}
