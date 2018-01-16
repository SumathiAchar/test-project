using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    /// <summary>
    /// Controller class for MedicareLabFeeSchedule PaymentType
    /// </summary>
    public class PaymentTypeMedicareLabFeeScheduleController : CommonController
    {
        /// <summary>
        /// Payments the type medicare lab fee schedule.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="serviceTypeId">The service type identifier.</param>
        /// <param name="paymentTypeId">The payment type identifier.</param>
        /// <param name="isEdit">if set to <c>true</c> [is edit].</param>
        /// <returns></returns>
        public ActionResult PaymentTypeMedicareLabFeeSchedule(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypeMedicareLabFeeSchedulePaymentViewModel paymentTypeMedicareLabFeeSchedulePaymentViewModel =
                new PaymentTypeMedicareLabFeeSchedulePaymentViewModel();

            if (isEdit)
            {
                PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedulePaymentForPost = new PaymentTypeMedicareLabFeeSchedule
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };
                //Get the Name of User logged in

                PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedule =
             PostApiResponse<PaymentTypeMedicareLabFeeSchedule>("PaymentTypeMedicareLabFeeSchedule",
                                                           "GetPaymentTypeMedicareLabFeeScheduleDetails",
                                                           paymentTypeMedicareLabFeeSchedulePaymentForPost);

                paymentTypeMedicareLabFeeSchedulePaymentViewModel = AutoMapper.Mapper.Map<PaymentTypeMedicareLabFeeSchedule, PaymentTypeMedicareLabFeeSchedulePaymentViewModel>(paymentTypeMedicareLabFeeSchedule);
            }

            paymentTypeMedicareLabFeeSchedulePaymentViewModel.ContractId = contractId;
            paymentTypeMedicareLabFeeSchedulePaymentViewModel.ServiceTypeId = serviceTypeId;
            paymentTypeMedicareLabFeeSchedulePaymentViewModel.PaymentTypeId = paymentTypeId;
            paymentTypeMedicareLabFeeSchedulePaymentViewModel.IsEdit = isEdit;
            return View(paymentTypeMedicareLabFeeSchedulePaymentViewModel);
        }

        /// <summary>
        /// Adds the edit payment type medicare lab fee schedule.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditPaymentTypeMedicareLabFeeSchedule(PaymentTypeMedicareLabFeeSchedulePaymentViewModel info)
        {
            PaymentTypeMedicareLabFeeSchedule paymentTypeMedicareLabFeeSchedule = AutoMapper.Mapper.Map<PaymentTypeMedicareLabFeeSchedulePaymentViewModel, PaymentTypeMedicareLabFeeSchedule>(info);
            //Get the Name of User logged in
            paymentTypeMedicareLabFeeSchedule.UserName = GetCurrentUserName();
            long medicareLabFeeScheduleId = PostApiResponse<long>("PaymentTypeMedicareLabFeeSchedule", "AddEditPaymentTypeMedicareLabFeeScheduleDetails", paymentTypeMedicareLabFeeSchedule);
            return medicareLabFeeScheduleId > 0 ? Json(new { sucess = true, Id = medicareLabFeeScheduleId }) : Json(new { sucess = false });
        }
    }
}
