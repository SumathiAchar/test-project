using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypeDrgController : CommonController
    {
        /// <summary>
        /// PaymentTypeDRGPaymentDetails
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="serviceTypeId"></param>
        /// <param name="paymentTypeId"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        public ActionResult PaymentTypeDrg(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypeDrgPaymentViewModel modelPaymentTypeDrgPaymentViewModel = new PaymentTypeDrgPaymentViewModel();
            if (isEdit)
            {
                PaymentTypeDrg paymentTypeDrgPaymentForPost = new PaymentTypeDrg
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };
                //Get the Name of User logged in

                PaymentTypeDrg paymentTypeDrgPaymentViewModelInfo =
             PostApiResponse<PaymentTypeDrg>("PaymentTypeDrg",
                                                           "GetPaymentTypeDrgPayment",
                                                           paymentTypeDrgPaymentForPost);

                modelPaymentTypeDrgPaymentViewModel = AutoMapper.Mapper.Map<PaymentTypeDrg, PaymentTypeDrgPaymentViewModel>(paymentTypeDrgPaymentViewModelInfo);
            }

            modelPaymentTypeDrgPaymentViewModel.ContractId = contractId;
            modelPaymentTypeDrgPaymentViewModel.ServiceTypeId = serviceTypeId;
            modelPaymentTypeDrgPaymentViewModel.PaymentTypeId = paymentTypeId;
            modelPaymentTypeDrgPaymentViewModel.IsEdit = isEdit;
            return View(modelPaymentTypeDrgPaymentViewModel);
        }

        /// <summary>
        /// Get the all Claim Fields selection basing on serviceTypeId
        /// </summary>
        [HttpGet]
        public JsonResult GetRelativeWeightSelection()
        {
            List<RelativeWeight> paymentRelativeWeight = GetApiResponse<List<RelativeWeight>>("PaymentTypeDrg", "GetAllRelativeWeightList");
            List<SelectListItem> paymentRelativeWeightList = new List<SelectListItem>();
            if (paymentRelativeWeight != null && paymentRelativeWeight.Count > 0)
            {
                paymentRelativeWeightList.AddRange(paymentRelativeWeight.Select(item => new SelectListItem { Text = item.RelativeWeightValue, Value = item.RelativeWeightId.ToString(CultureInfo.InvariantCulture) }));
            }
            return Json(new { relativeWeightList = paymentRelativeWeightList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the relative weight selection.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetClaimFields()
        {
            // Get Claim field from DB 
            List<ClaimField> contractClaimFeilds = GetApiResponse<List<ClaimField>>("ClaimFieldDoc", "GetAllClaimFields");
            List<SelectListItem> contractServiceClaims = new List<SelectListItem>();
            if (contractClaimFeilds != null && contractClaimFeilds.Count > 0)
            {
                contractServiceClaims.AddRange(contractClaimFeilds.Select(item => new SelectListItem { Text = item.Text, Value = item.ClaimFieldId.ToString(CultureInfo.InvariantCulture) }));
            }
            //Removing ClainId from Claimfields Dropdown
            contractServiceClaims.RemoveAll(a => a.Text == "Adjudication Request Name" || a.Text == "ClaimID");
            return Json(new { claimFeildList = contractServiceClaims });
        }

        /// <summary>
        /// Adds the payment type DRG.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditPaymentTypeDrg(PaymentTypeDrgPaymentViewModel info)
        {
            long drgId = 0;
            if (info.ClaimFieldDocId != null)
            {
                PaymentTypeDrg drgPaymrntInfo =
                    AutoMapper.Mapper.Map<PaymentTypeDrgPaymentViewModel, PaymentTypeDrg>(info);
                //Get the Name of User logged in
                drgPaymrntInfo.UserName = GetCurrentUserName();
                drgId = PostApiResponse<long>("PaymentTypeDrg", "AddEditPaymentTypeDrgPayment", drgPaymrntInfo);
            }
            return drgId > 0 ? Json(new { sucess = true, Id = drgId, documentId = info.ClaimFieldDocId }) : Json(new { sucess = false, Id = 0, documentId = info.ClaimFieldDocId });
        }
    }
}
