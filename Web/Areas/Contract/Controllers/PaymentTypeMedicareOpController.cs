using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypeMedicareOpController : CommonController
    {
        //
        // GET: /PaymentTypeMedicareOPDetails/

        public ActionResult PaymentTypeMedicareOp(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypeMedicareOpPaymentViewModel modePaymentTypeMedicareOpPaymentViewModel = new PaymentTypeMedicareOpPaymentViewModel();
            if (isEdit)
            {
                PaymentTypeMedicareOp paymentTypeMedicareOpPaymentForPost = new PaymentTypeMedicareOp
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };
                //Get the Name of User logged in

                PaymentTypeMedicareOp medicareOpInfo =
             PostApiResponse<PaymentTypeMedicareOp>("PaymentTypeMedicareOp",
                                                           "GetPaymentTypeMedicareOpDetails",
                                                           paymentTypeMedicareOpPaymentForPost);

                modePaymentTypeMedicareOpPaymentViewModel = AutoMapper.Mapper.Map<PaymentTypeMedicareOp, PaymentTypeMedicareOpPaymentViewModel>(medicareOpInfo);
            }

            modePaymentTypeMedicareOpPaymentViewModel.ContractId = contractId;
            modePaymentTypeMedicareOpPaymentViewModel.ServiceTypeId = serviceTypeId;
            modePaymentTypeMedicareOpPaymentViewModel.PaymentTypeId = paymentTypeId;
            modePaymentTypeMedicareOpPaymentViewModel.IsEdit = isEdit;
            return View(modePaymentTypeMedicareOpPaymentViewModel);
        }

        /// <summary>
        /// Adds the payment type medicare op.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditPaymentTypeMedicareOp(PaymentTypeMedicareOpPaymentViewModel info)
        {
            PaymentTypeMedicareOp medicareOpPaymrntInfo = AutoMapper.Mapper.Map<PaymentTypeMedicareOpPaymentViewModel, PaymentTypeMedicareOp>(info);
            //Get the Name of User logged in
            medicareOpPaymrntInfo.UserName = GetCurrentUserName();
            long medicareOpId = PostApiResponse<long>("PaymentTypeMedicareOp", "AddEditPaymentTypeMedicareOpDetails", medicareOpPaymrntInfo);
            return medicareOpId > 0 ? Json(new { sucess = true, Id = medicareOpId }) : Json(new { sucess = false });
        }
    }
}
