using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypePerVisitController : CommonController
    {
        //
        // GET: /PaymentTypePerVisitDetails/

        public ActionResult PaymentTypePerVisit(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypePerVisitViewModel modePaymentTypePerVisitViewModel = new PaymentTypePerVisitViewModel();
            if (isEdit)
            {
                PaymentTypePerVisit paymentTypePerVisitForPost = new PaymentTypePerVisit
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };
                //Get the Name of User logged in
                PaymentTypePerVisit paymentTypePerVisitViewModelInfo =
             PostApiResponse<PaymentTypePerVisit>("PaymentTypePerVisit",
                                                           "GetPaymentTypePerVisitDetails",
                                                           paymentTypePerVisitForPost);

                modePaymentTypePerVisitViewModel = AutoMapper.Mapper.Map<PaymentTypePerVisit, PaymentTypePerVisitViewModel>(paymentTypePerVisitViewModelInfo);
            }

            modePaymentTypePerVisitViewModel.ContractId = contractId;
            modePaymentTypePerVisitViewModel.ServiceTypeId = serviceTypeId;
            modePaymentTypePerVisitViewModel.PaymentTypeId = paymentTypeId;
            modePaymentTypePerVisitViewModel.IsEdit = isEdit;
            return View(modePaymentTypePerVisitViewModel);
        }

        /// <summary>
        /// Add Edit the payment per visit.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditPaymentPerVisit(PaymentTypePerVisitViewModel info)
        {
            PaymentTypePerVisit perVisistPaymentInfo = AutoMapper.Mapper.Map<PaymentTypePerVisitViewModel, PaymentTypePerVisit>(info);
            //Get the Name of User logged in
            perVisistPaymentInfo.UserName = GetCurrentUserName();
            long pervisitId = PostApiResponse<long>("PaymentTypePerVisit", "AddEditPaymentTypePerVisitDetails", perVisistPaymentInfo);
            return pervisitId > 0 ? Json(new { sucess = true, Id = pervisitId }) : Json(new { sucess = false });
        }
    }
}
