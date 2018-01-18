using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers.MWAppServices;
using SSI.ContractManagement.Shared.Helpers.MWServiceUtil;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Models;

namespace SSI.ContractManagement.Web.Controllers
{
    //FIXED-MAY If the underlying object is called XXX, the associated classes (api.service, logic, repository) are called in sync as XXXXLogic, XXXXController, XXXXRepository
    // etc. In that sense, this should be called as ContractModelController. Also, class names should not be VERBs.
    public class ContractModelController : BaseController
    {
        /// <summary>
        /// Get the all Contract Models
        /// </summary>
        [HttpPost]
        public JsonResult GetAllContractModels(long facilityId)
        {
            SecurityModel security = new SecurityModel();
            ContractModel data = new ContractModel {UserName = security.CurrentUserName, FacilityID = facilityId};

            // Checking if the user has AllContractAccess, that mean he is a admin or super admin.
            bool isCMSAccessAllContractPrivilegeFound = HasPermissions(UserInfo.Privilege.CMSAccessAllContract);
            if (isCMSAccessAllContractPrivilegeFound)
            {
                data.UserName = null;
            }
            List<ContractModel> contractModels = PostApiResponse<List<ContractModel>>("ContractModel",
                "GetAllContractModels", data);
            List<SelectListItem> contractModelsList = new List<SelectListItem>();
            if (contractModels != null && contractModels.Count > 0)
            {
                contractModelsList.AddRange(contractModels.Select(item => new SelectListItem
                {
                    Text = item.NodeText,
                    Value = item.NodeId.ToString(CultureInfo.InvariantCulture)
                }));
            }
            return Json(new { contractModelList = contractModelsList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get the all All Facilities
        /// </summary>
        public JsonResult GetAllFacilities()
        {
            ContractHierarchy data = new ContractHierarchy();
            List<UserAccessModel> userFacilities = GetUserAccessZones();
            if (userFacilities !=    null && userFacilities.Any())
            {
                data.FacilityList = userFacilities.Select(a => a.Id).ToList();
            }
            List<FacilityDetail> contractFacility = PostApiResponse<List<FacilityDetail>>("ContractModel", "GetAllContractFacilities", data);

            return Json(new { FacilityList = contractFacility }, JsonRequestBehavior.AllowGet);
        }
    }
}
