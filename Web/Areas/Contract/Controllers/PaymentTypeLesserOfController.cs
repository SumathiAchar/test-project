using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypeLesserOfController : CommonController
    {
        /// <summary>
        /// Payments the type lesser of.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="serviceTypeId">The service type identifier.</param>
        /// <param name="paymentTypeId">The payment type identifier.</param>
        /// <param name="isEdit">if set to <c>true</c> [is edit].</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PaymentTypeLesserOf(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            //To show default percentage as 100
            PaymentTypeLesserOfViewModel paymentTypeLesserOfViewModel = new PaymentTypeLesserOfViewModel
            {
                Percentage = Constants.PaymentTypeLesserOfPercentage
            };
            if (isEdit)
            {
                PaymentTypeLesserOf paymentTypeLesserOfForPost = new PaymentTypeLesserOf
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };

              //Get the Name of User logged in
              PaymentTypeLesserOf paymentTypeLesserOf = PostApiResponse<PaymentTypeLesserOf>("PaymentTypeLesserOf",
                    "GetLesserOfPercentage", paymentTypeLesserOfForPost);
                paymentTypeLesserOfViewModel = AutoMapper.Mapper.Map<PaymentTypeLesserOf, PaymentTypeLesserOfViewModel>(paymentTypeLesserOf);
            }
            paymentTypeLesserOfViewModel.PaymentTypeId = (byte)Enums.PaymentTypeCodes.LesserOf;
            paymentTypeLesserOfViewModel.ServiceTypeId = serviceTypeId;
            paymentTypeLesserOfViewModel.ContractId = contractId;
            paymentTypeLesserOfViewModel.IsEdit = isEdit;
            return View(paymentTypeLesserOfViewModel);
        }

        /// <summary>
        /// Adds the edit payment type lesser of.
        /// </summary>
        /// <param name="paymentTypeLesserOfViewModel">The payment type lesser of view model.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditPaymentTypeLesserOf(PaymentTypeLesserOfViewModel paymentTypeLesserOfViewModel)
        {
            //Get the Name of User logged in
            paymentTypeLesserOfViewModel.UserName = GetCurrentUserName();
            PaymentTypeLesserOf paymentTypeLesserOf =
                AutoMapper.Mapper.Map<PaymentTypeLesserOfViewModel, PaymentTypeLesserOf>(paymentTypeLesserOfViewModel);
            long paymentTypeLesserOfId = PostApiResponse<long>("PaymentTypeLesserOf", "AddEditPaymentTypeLesserOf", paymentTypeLesserOf);
            return paymentTypeLesserOfId > 0 ? Json(new { success = true}) : Json(new { sucess = false });
        }
    }
}
