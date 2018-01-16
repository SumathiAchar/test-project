using SSI.ContractManagement.Shared.Helpers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Web.Areas.Common.Controllers
{
    
    public class PayerController : CommonController
    {
        /// <summary>
        /// Gets the payers.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="serviceTypeId">The service type identifier.</param>
        /// <param name="userText"></param>
        /// <returns></returns>
        public JsonResult GetPayers(long? contractId, long? serviceTypeId, string userText)
        {
            int facilityId = GetCurrentFacilityId();
            ContractServiceLineClaimFieldSelection selectedFacility = new ContractServiceLineClaimFieldSelection
            {
                FacilityId = facilityId,
                ContractId = contractId,
                ContractServiceTypeId = serviceTypeId,
                Values = userText
            };
            if (facilityId != 0)
            {
                selectedFacility.SsiNumber = GetSsiNumberBasedOnFacilityId(facilityId);
            }
            List<Payer> payerListDetails = PostApiResponse<List<Payer>>(Constants.Payer, Constants.GetPayers, selectedFacility);
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.AddRange(
                            payerListDetails.Select(
                                requesteritem =>
                                    new SelectListItem
                                    {
                                        Value = requesteritem.PayerId.ToString(CultureInfo.InvariantCulture),
                                        Text = requesteritem.PayerName
                                    }));
            return Json(selectListItems, JsonRequestBehavior.AllowGet);
        }
    }
}
