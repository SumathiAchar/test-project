using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Web.Areas.Common.Controllers
{
    public class ClaimFieldController : CommonController
    {
        /// <summary>
        /// Gets claim fields based on module.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetClaimFieldsByModule(int? moduleId)
        {
            // Get Claim field from DB 
            List<ClaimField> claimFeilds = GetApiResponse<List<ClaimField>>("ClaimField", "GetClaimFieldsByModule", moduleId);
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (claimFeilds != null && claimFeilds.Count > 0)
            {
                listItems.AddRange(
                    claimFeilds.Select(
                        item =>
                            new SelectListItem
                            {
                                Text = item.Text,
                                Value = item.ClaimFieldId.ToString(CultureInfo.InvariantCulture)
                            }));
            }
            return Json(new { claimFeildList = listItems }, JsonRequestBehavior.AllowGet);
        }    
    }
}