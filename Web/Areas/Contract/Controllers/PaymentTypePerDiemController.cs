using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypePerDiemController : CommonController
    {
        //
        // GET: /PaymentTypePerDiemDetails/

        public ActionResult PaymentTypePerDiem(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypePerDiemViewModel modelPaymentTypePerDiemViewModel = new PaymentTypePerDiemViewModel();
            if (isEdit)
            {
                PaymentTypePerDiem paymentTypePerDiemPost = new PaymentTypePerDiem
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };

                //Get the Name of User logged in
                PaymentTypePerDiem paymentTypePerDiemList =
             PostApiResponse<PaymentTypePerDiem>("PaymentTypePerDiem",
                                                           "GetPaymentTypePerDiem",
                                                           paymentTypePerDiemPost);

                modelPaymentTypePerDiemViewModel = AutoMapper.Mapper.Map<PaymentTypePerDiem, PaymentTypePerDiemViewModel>(paymentTypePerDiemList);
            }

            modelPaymentTypePerDiemViewModel.ContractId = contractId;
            modelPaymentTypePerDiemViewModel.ServiceTypeId = serviceTypeId;
            modelPaymentTypePerDiemViewModel.PaymentTypeId = paymentTypeId;
            modelPaymentTypePerDiemViewModel.IsEdit = isEdit;
            return View(modelPaymentTypePerDiemViewModel);
        }

        /// <summary>
        /// Saves the specified list of payment type per diem.
        /// </summary>
        /// <param name="listofPaymentTypePerDiem">The list of payment type per diem.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save(PaymentTypePerDiemViewModel listofPaymentTypePerDiem)
        {
            PaymentTypePerDiem paymentTypeModel = AutoMapper.Mapper.Map<PaymentTypePerDiemViewModel, PaymentTypePerDiem>(listofPaymentTypePerDiem);
            //Get the Name of User logged in
            paymentTypeModel.UserName = GetCurrentUserName();
            long noOfRecord = PostApiResponse<long>("PaymentTypePerDiem", "AddEditPaymentTypePerDiem", paymentTypeModel);
            return noOfRecord > 0 ? Json(new { sucess = true, Id = noOfRecord }) : Json(new { sucess = false });
        }

    }
}
