using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypePerCaseController : CommonController
    {
        //
        // GET: /PaymentTypePerCaseDetails/
        public ActionResult PaymentTypePerCase(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypePerCaseViewModel modelPaymentTypePerCaseViewModel = new PaymentTypePerCaseViewModel();
            if (isEdit)
            {
                PaymentTypePerCase paymentTypePerCaseForPost = new PaymentTypePerCase
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };
                //Get the Name of User logged in

                PaymentTypePerCase paymentTypePerCaseViewModelInfo =
             PostApiResponse<PaymentTypePerCase>("PaymentTypePerCase",
                                                           "GetPaymentPerCaseDetails",
                                                           paymentTypePerCaseForPost);

                modelPaymentTypePerCaseViewModel = AutoMapper.Mapper.Map<PaymentTypePerCase, PaymentTypePerCaseViewModel>(paymentTypePerCaseViewModelInfo);
            }

            modelPaymentTypePerCaseViewModel.ContractId = contractId;
            modelPaymentTypePerCaseViewModel.ServiceTypeId = serviceTypeId;
            modelPaymentTypePerCaseViewModel.PaymentTypeId = paymentTypeId;
            modelPaymentTypePerCaseViewModel.IsEdit = isEdit;
            return View(modelPaymentTypePerCaseViewModel);
        }

        /// <summary>
        /// Adds the edit payment per case.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditPaymentPerCase(PaymentTypePerCaseViewModel info)
        {
            PaymentTypePerCase perCasePaymentInfo = AutoMapper.Mapper.Map<PaymentTypePerCaseViewModel, PaymentTypePerCase>(info);
            //Get the Name of User logged in
            perCasePaymentInfo.UserName = GetCurrentUserName();
            long perCaseId = PostApiResponse<long>("PaymentTypePerCase", "AddEditPaymentPerCaseDetails", perCasePaymentInfo);
            return perCaseId > 0 ? Json(new { sucess = true, Id = perCaseId }) : Json(new { sucess = false });
        }
    }
}
