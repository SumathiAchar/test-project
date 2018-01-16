using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypeCapController : CommonController
    {
        /// <summary>
        /// Payments the type cap details.
        /// </summary>
        /// <param name="contractId">The contract unique identifier.</param>
        /// <param name="serviceTypeId">The service type unique identifier.</param>
        /// <param name="paymentTypeId">The payment type unique identifier.</param>
        /// <param name="isEdit">if set to <c>true</c> [is edit].</param>
        /// <returns></returns>
        public ActionResult PaymentTypeCap(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypeCapViewModel modelPaymentTypeCapViewModel = new PaymentTypeCapViewModel();
            if (isEdit)
            {
                PaymentTypeCap paymentTypeCapForPost = new PaymentTypeCap
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };

                //Get the Name of User logged in
                PaymentTypeCap medicareIpInfo =
             PostApiResponse<PaymentTypeCap>("PaymentTypeCap",
                                                           "GetPaymentTypeCap",
                                                           paymentTypeCapForPost);

                modelPaymentTypeCapViewModel = AutoMapper.Mapper.Map<PaymentTypeCap, PaymentTypeCapViewModel>(medicareIpInfo);
            }

            modelPaymentTypeCapViewModel.ContractId = contractId;
            modelPaymentTypeCapViewModel.ServiceTypeId = serviceTypeId;
            modelPaymentTypeCapViewModel.PaymentTypeId = paymentTypeId;
            modelPaymentTypeCapViewModel.IsEdit = isEdit;
            return View(modelPaymentTypeCapViewModel);
        }

        /// <summary>
        /// Adds the payment type cap.
        /// </summary>
        /// <param name="paymentTypeCapInfo">The payment type cap information.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult AddEditPaymentTypeCap(PaymentTypeCapViewModel paymentTypeCapInfo)
        {
            PaymentTypeCap pTypeCaP = AutoMapper.Mapper.Map<PaymentTypeCapViewModel, PaymentTypeCap>(paymentTypeCapInfo);
            //Get the Name of User logged in
            pTypeCaP.UserName = GetCurrentUserName();
            long pTypeCapId = PostApiResponse<long>("PaymentTypeCap", "AddEditPaymentTypeCap", pTypeCaP);
            return pTypeCapId > 0 ? Json(new { sucess = true, Id = pTypeCapId }) : Json(new { sucess = false });
        }
    }
}
