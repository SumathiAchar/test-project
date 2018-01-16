using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypeMedicareIpController : CommonController
    {
        /// <summary>
        /// Payments the type Medicare ip.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="serviceTypeId">The service type identifier.</param>
        /// <param name="paymentTypeId">The payment type identifier.</param>
        /// <param name="isEdit">if set to <c>true</c> [is edit].</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PaymentTypeMedicareIp(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypeMedicareIpPaymentViewModel paymentTypeMedicareIpViewModel =
                new PaymentTypeMedicareIpPaymentViewModel();

            // Getting the list of Medicare ip options
            List<MedicareIpAcuteOption> medicareIpAcuteOptions =
                GetApiResponse<List<MedicareIpAcuteOption>>(Constants.MedicareIpAcuteOption, Constants.GetMedicareIpAcuteOptions);
            if (isEdit)
            {
                PaymentTypeMedicareIp paymentTypeMedicareIp = new PaymentTypeMedicareIp
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    //Get the Name of User logged in
                    UserName = GetCurrentUserName()
                };

                paymentTypeMedicareIpViewModel =
                    AutoMapper.Mapper.Map<PaymentTypeMedicareIp, PaymentTypeMedicareIpPaymentViewModel>(
                        PostApiResponse<PaymentTypeMedicareIp>(Constants.PaymentTypeMedicareIp,
                            Constants.GetPaymentTypeMedicareIpDetails,
                            paymentTypeMedicareIp));
            }

            paymentTypeMedicareIpViewModel.ContractId = contractId;
            paymentTypeMedicareIpViewModel.ServiceTypeId = serviceTypeId;
            paymentTypeMedicareIpViewModel.PaymentTypeId = paymentTypeId;
            paymentTypeMedicareIpViewModel.IsEdit = isEdit;
            paymentTypeMedicareIpViewModel.MedicareIpAcuteOptions = new List<MedicareIpAcuteOptionViewModel>();

            foreach (MedicareIpAcuteOption medicareIpAcuteOption in medicareIpAcuteOptions)
            {
                paymentTypeMedicareIpViewModel.MedicareIpAcuteOptions.Add(
                    AutoMapper.Mapper.Map<MedicareIpAcuteOption, MedicareIpAcuteOptionViewModel>(medicareIpAcuteOption));
            }
            return View(paymentTypeMedicareIpViewModel);
        }

        /// <summary>
        /// Adds the payment type Medicare ip.
        /// </summary>
        /// <param name="paymentTypeMedicareIp">The information.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditPaymentTypeMedicareIp(PaymentTypeMedicareIpPaymentViewModel paymentTypeMedicareIp)
        {
            PaymentTypeMedicareIp medicareIpPayment = AutoMapper.Mapper.Map<PaymentTypeMedicareIpPaymentViewModel, PaymentTypeMedicareIp>(paymentTypeMedicareIp);
            //Get the Name of User logged in
            medicareIpPayment.UserName = GetCurrentUserName();
            long medicareIpId = PostApiResponse<long>(Constants.PaymentTypeMedicareIp, Constants.AddEditPaymentTypeMedicareIpDetails, medicareIpPayment);
            return medicareIpId > 0 ? Json(new { sucess = true, Id = medicareIpId }) : Json(new { sucess = false, Id = 0 });
        }
    }
}
