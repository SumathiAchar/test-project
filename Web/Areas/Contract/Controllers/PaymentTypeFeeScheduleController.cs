using System.Web.Mvc;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypeFeeScheduleController : CommonController
    {

        /// <summary>
        /// Payments the type fee schedule details.
        /// </summary>
        /// <param name="contractId">The contract unique identifier.</param>
        /// <param name="serviceTypeId">The service type unique identifier.</param>
        /// <param name="paymentTypeId">The payment type unique identifier.</param>
        /// <param name="isEdit">if set to <c>true</c> [is edit].</param>
        /// <returns></returns>
        public ActionResult PaymentTypeFeeSchedule(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypeFeeSchedulesViewModel modelPaymentTypeFeeSchedulesViewMode = new PaymentTypeFeeSchedulesViewModel();
            if (isEdit)
            {
                PaymentTypeFeeSchedule paymentTypeFeeSchedulesForPost = new PaymentTypeFeeSchedule
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()

                };


                PaymentTypeFeeSchedule paymentTypeFeeSchedulesViewModelInfo =
             PostApiResponse<PaymentTypeFeeSchedule>("PaymentTypeFeeSchedule",
                                                           "GetPaymentTypeFeeSchedule",
                                                           paymentTypeFeeSchedulesForPost);

                modelPaymentTypeFeeSchedulesViewMode = AutoMapper.Mapper.Map<PaymentTypeFeeSchedule, PaymentTypeFeeSchedulesViewModel>(paymentTypeFeeSchedulesViewModelInfo);
            }
            modelPaymentTypeFeeSchedulesViewMode.ContractId = contractId;
            modelPaymentTypeFeeSchedulesViewMode.ServiceTypeId = serviceTypeId;
            modelPaymentTypeFeeSchedulesViewMode.PaymentTypeId = paymentTypeId;
            modelPaymentTypeFeeSchedulesViewMode.IsEdit = isEdit;
            return View(modelPaymentTypeFeeSchedulesViewMode);
        }

        /// <summary>
        /// Adds the payment type fee schedule.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult AddEditPaymentTypeFeeSchedule(PaymentTypeFeeSchedulesViewModel info)
        {
            long paymentTypeFeeScheduleId = 0;
            if (info.ClaimFieldDocId != null)
            {
                PaymentTypeFeeSchedule feeSchedule =
                    AutoMapper.Mapper.Map<PaymentTypeFeeSchedulesViewModel, PaymentTypeFeeSchedule>(info);

                feeSchedule.UserName = GetCurrentUserName();

                paymentTypeFeeScheduleId = PostApiResponse<long>("PaymentTypeFeeSchedule",
                    "AddEditPaymentTypeFeeSchedule", feeSchedule);
            }
            return paymentTypeFeeScheduleId > 0 ? Json(new { sucess = true, Id = paymentTypeFeeScheduleId, documentId = info.ClaimFieldDocId }) : Json(new { sucess = false, documentId = info.ClaimFieldDocId });
        }
    }
}

