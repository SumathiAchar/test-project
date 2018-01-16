using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypePercentageChargeController : CommonController
    {
        //
        // GET: /PaymentTypePercentageDiscountDetails/

        public ActionResult PaymentTypePercentageCharge(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypePercentageChargeViewModel modePaymentTypePercentageDiscountViewModel = new PaymentTypePercentageChargeViewModel();
            if (isEdit)
            {
                PaymentTypePercentageCharge paymentTypePercentageDiscountForPost = new PaymentTypePercentageCharge
                {
                    ServiceTypeId =
                        serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId =
                        paymentTypeId,
                    UserName = GetCurrentUserName()
                };

                //Get the Name of User logged in
                PaymentTypePercentageCharge paymentTypePercentageDiscountInfo =
             PostApiResponse<PaymentTypePercentageCharge>("PaymentTypePercentageCharge",
                                                           "GetPaymentTypePercentageDiscount",
                                                           paymentTypePercentageDiscountForPost);

                modePaymentTypePercentageDiscountViewModel = AutoMapper.Mapper.Map<PaymentTypePercentageCharge, PaymentTypePercentageChargeViewModel>(paymentTypePercentageDiscountInfo);
            }

            modePaymentTypePercentageDiscountViewModel.ContractId = contractId;
            modePaymentTypePercentageDiscountViewModel.ServiceTypeId = serviceTypeId;
            modePaymentTypePercentageDiscountViewModel.PaymentTypeId = paymentTypeId;
            modePaymentTypePercentageDiscountViewModel.IsEdit = isEdit;
            return View(modePaymentTypePercentageDiscountViewModel);
        }

        /// <summary>
        /// Adds the edit percentage discount details.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditPercentageDiscountDetails(PaymentTypePercentageChargeViewModel info)
        {
            PaymentTypePercentageCharge percentagePaymentInfo = AutoMapper.Mapper.Map<PaymentTypePercentageChargeViewModel, PaymentTypePercentageCharge>(info);
            //Get the Name of User logged in
            percentagePaymentInfo.UserName = GetCurrentUserName();
            long percentageId = PostApiResponse<long>("PaymentTypePercentageCharge", "AddEditPaymentTypePercentageDiscount", percentagePaymentInfo);
            return percentageId > 0 ? Json(new { sucess = true, Id = percentageId }) : Json(new { sucess = false });
        }
    }
}
