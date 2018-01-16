using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ContractModelController : CommonController
    {
        /// <summary>
        /// Get the all Contract Models
        /// </summary>
        [HttpPost]
        public JsonResult GetAllContractModels()
        {
            ContractModel data = new ContractModel { UserName = GetCurrentUserName(), FacilityId = GetCurrentFacilityId() };
            
            List<ContractModel> contractModels = PostApiResponse<List<ContractModel>>(Constants.ContractModel,
                Constants.GetAllContractModels, data);
            List<SelectListItem> contractModelsList = new List<SelectListItem>();
            if (contractModels != null && contractModels.Count > 0)
            {
                contractModelsList.AddRange(contractModels.Select(item => new SelectListItem
                {
                    Text = item.NodeText,
                    Value = item.NodeId.ToString(CultureInfo.InvariantCulture)
                }));
            }
            return Json(new { contractModelList = contractModelsList });
        }
    }
}
