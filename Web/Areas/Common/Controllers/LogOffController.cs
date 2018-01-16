using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;
using Constants = SSI.ContractManagement.Shared.Helpers.Constants;

namespace SSI.ContractManagement.Web.Areas.Common.Controllers
{
    public class LogOffController : CommonController
    {
        //
        // GET: /Common/LogOff/

        /// <summary>
        /// Indexes the specified is session time out.
        /// </summary>
        /// <param name="isSessionTimeOut">if set to <c>true</c> [is session time out].</param>
        /// <returns></returns>
        ///FIXED-NOV15--Use Constants and area as Common
        public ActionResult Index(bool isSessionTimeOut)
        {
            UserInfo userInfoViewModel = GetUserInfo();
            //Audit logging for FacilityAdmin and User login but not for SSIAdmin login (UserTypeId = 1)
            if (userInfoViewModel != null && userInfoViewModel.AssignedFacilities != null && userInfoViewModel.AssignedFacilities.Any() && userInfoViewModel.UserTypeId != Constants.One)
            {
                List<FacilityViewModel> facilityDetails = GetAssignedFacilities().ToList();
                List<Facility> facilities = AutoMapper.Mapper.Map<List<FacilityViewModel>, List<Facility>>(facilityDetails);

                LogOff logOffInfo = new LogOff
                {
                    UserName = userInfoViewModel.UserName,
                    IsSessionTimeOut = isSessionTimeOut,
                    UserFacilities = facilities 
                };
                //Save Logout/Session Timeout Audit Log for Contract Management
                PostApiResponse<bool>(Constants.LogOff, Constants.InsertAuditLog,
                    logOffInfo, true);
            }
            if (isSessionTimeOut)
            {
                AuditLogReport auditLog = new AuditLogReport
                {
                    RequestedUserName = GetLoggedInUserName(),
                    Description = Constants.UserSessionTimedOut
                };
                //Save Logout Audit Log
                PostApiResponse<int>(Constants.User, Constants.SaveAuditLog, auditLog, true);
            }

            return RedirectToAction(Constants.LogOff, Constants.Account, new { area = Constants.Common });
        }

    }
}
