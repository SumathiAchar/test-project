using System.Web.Mvc;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.UserManagement.Controllers
{
    public class HomePageController : BaseController
    {
        /// <summary>
        /// View Facility And User Home Page
        /// </summary>
        public ActionResult FacilityAndUserHomePage()
        {
            UserInfo userinfo = GetUserInfo();
            ViewBag.UserName = userinfo.UserName;
            ViewBag.UserId = userinfo.UserId;
            ViewBag.UserTypeId = userinfo.UserTypeId;
            ViewBag.LastLoginDate = userinfo.LastLoginDate;
            ViewBag.PasswordExpirationDays = userinfo.PasswordExpirationDays;

            return View();
        }
    }
}
