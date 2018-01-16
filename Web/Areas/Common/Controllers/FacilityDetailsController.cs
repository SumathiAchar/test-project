
/************************************************************************************************************/
/**  Author         : Vishesh Bhawsar
/**  Created        : 2-Sep-2013
/**  Summary        : Handles Facility Details
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   : 
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Web.Areas.Common.Controllers
{
    public class FacilityDetailsController : SessionStoreController
    {
        /// <summary>
        /// Get the all All Facilities
        /// </summary>
        public JsonResult GetAllFacilities()
        {
            ContractHierarchy data = new ContractHierarchy();
           IEnumerable<int> facilities = GetUserFacilityIds();
            if (facilities != null && facilities.Any())
            {
                data.FacilityList = (List<int>) GetUserFacilityIds();
            }
            List<FacilityDetail> contractFacility = PostApiResponse<List<FacilityDetail>>(Constants.ContractModel, Constants.GetAllContractFacilities, data);
            return Json(new { FacilityList = contractFacility }, JsonRequestBehavior.AllowGet);
        }
    }
}