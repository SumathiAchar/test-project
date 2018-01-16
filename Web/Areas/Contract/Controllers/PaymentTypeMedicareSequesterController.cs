using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypeMedicareSequesterController : CommonController
    {
        /// <summary>
        /// Get Payment Type Medicare Sequester
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="serviceTypeId"></param>
        /// <param name="paymentTypeId"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public ActionResult PaymentTypeMedicareSequester(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypeMedicareSequesterViewModel paymentTypeMedicareSequesterViewModel = new PaymentTypeMedicareSequesterViewModel();
            if (isEdit)
            {
                PaymentTypeMedicareSequester paymentTypeMedicareSequester = new PaymentTypeMedicareSequester
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };
                PaymentTypeMedicareSequester paymentTypeMedicareSequesteViewModelInfo =
                    PostApiResponse<PaymentTypeMedicareSequester>(Constants.PaymentTypeMedicareSequester, Constants.GetPaymentMedicareSequester, paymentTypeMedicareSequester);

                paymentTypeMedicareSequesterViewModel =
                    AutoMapper.Mapper.Map<PaymentTypeMedicareSequester, PaymentTypeMedicareSequesterViewModel>(paymentTypeMedicareSequesteViewModelInfo);
            }

            paymentTypeMedicareSequesterViewModel.ContractId = contractId;
            paymentTypeMedicareSequesterViewModel.ServiceTypeId = serviceTypeId;
            paymentTypeMedicareSequesterViewModel.PaymentTypeId = paymentTypeId;
            paymentTypeMedicareSequesterViewModel.IsEdit = isEdit;
           
            return View(paymentTypeMedicareSequesterViewModel);
        }

        /// <summary>
        /// Adds the edit payment Medicare Sequester.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditPaymentMedicareSequester(PaymentTypeMedicareSequesterViewModel info)
        {
            PaymentTypeMedicareSequester medicareSequesterPaymentInfo = AutoMapper.Mapper.Map<PaymentTypeMedicareSequesterViewModel, PaymentTypeMedicareSequester>(info);
            //Get the Name of User logged in
            medicareSequesterPaymentInfo.UserName = GetCurrentUserName();
            long medicareSequesterId = PostApiResponse<long>(Constants.PaymentTypeMedicareSequester, Constants.AddEditPaymentMedicareSequester, medicareSequesterPaymentInfo);
            return medicareSequesterId > 0 ? Json(new { sucess = true, Id = medicareSequesterId }) : Json(new { sucess = false });
        }

    }
}
