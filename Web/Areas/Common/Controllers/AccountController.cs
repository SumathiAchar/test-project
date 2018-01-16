using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Web.ActionFilters;
using SSI.ContractManagement.Web.Areas.Common.Models;
using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;


namespace SSI.ContractManagement.Web.Areas.Common.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private string _currentDatetime = string.Empty;
        // ReSharper disable once InconsistentNaming
        bool isFromSecurityPage;
        /// <summary>
        /// Logins the specified return URL.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            RemoveSessionVariables();
            ViewBag.ReturnUrl = returnUrl;
            return View("~/Areas/Common/Views/Account/Login.cshtml");
        }

        /// <summary>
        /// Logins the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [RedirectOnError]
// ReSharper disable once RedundantAssignment
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            RemoveSessionVariables();
            returnUrl = null;
            _currentDatetime = Request.Form[Constants.CurrentDatetime];
            if (model.UserName == null || model.Password == null)
            {
                // If anyone of this is null, then it is invalid data for us. Need to send control back to UI with message.
                return View("~/Areas/Common/Views/Account/Login.cshtml", model);
            }
            model.UserName = model.UserName.ToLower();
            try
            {
                User userModel = new User { Email = model.UserName };

                //get user details
                User userDetails = PostApiResponse<User>(Constants.User, Constants.GetUserDetails, userModel, true);

                // validate the user
                if (userDetails != null && userDetails.UserId != 0)
                {
                    if (!userDetails.IsLocked)
                    {
                        if (userDetails.IsPasswordExpired) //password expired
                        {
                            ViewBag.IsPasswordExpired = true;
                            ViewBag.UserID = userDetails.UserId;
                            return View("~/Areas/Common/Views/Account/Login.cshtml", model);
                        }
                        if (UserValidation(model.Password, userDetails))
                        {
                            Session[Constants.IsUserLoggedIn] = true;
// ReSharper disable once ExpressionIsAlwaysNull
                            return RedirectToAction(Constants.SetSession, Constants.SessionStore, new { returnUrl, isFromSecurityPage });
                        }
                    }
                    else
                    {
                        //user is locked or reached maximum attempts
                        ViewBag.AccountLocked = true;
                        return View("~/Areas/Common/Views/Account/Login.cshtml", model);
                    }

                }
                // When user is null means user name or password is not correct.
                ViewBag.InvalidAccount = true;
                return View("~/Areas/Common/Views/Account/Login.cshtml", model);
            }
            catch (Exception ex)
            {
                //FIXED-RAGINI-FEB16 - Correct spellings
                //Added below code to check production login issue
                Log.LogError("Login Exception", "", ex);
                ModelState.AddModelError("", "Exception occurred while login. Please try again.");
                return View("~/Areas/Common/Views/Account/Login.cshtml", model);
            }
        }

        /// <summary>
        /// Account Activation Login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [RedirectOnError]
        public ActionResult AccountActivationLogin(LoginModel model, string currentDateTime, string returnUrl)
        {
            _currentDatetime = currentDateTime;
            RemoveSessionVariables();
            model.UserName = model.UserName.ToLower();
            User userModel = new User { Email = model.UserName };
           
            //get user details
            User userDetails = PostApiResponse<User>(Constants.User, Constants.GetUserDetails, userModel, true);

            // validate the user
            if (userDetails != null && userDetails.UserId != 0)
            {
                if (UserValidation(model.Password, userDetails))
                {
                    Session[Constants.IsUserLoggedIn] = true;
                    isFromSecurityPage = true;
                    return RedirectToAction(Constants.SetSession, Constants.SessionStore, new { returnUrl, isFromSecurityPage });
                }
            }
            return Json(true);
        }



        /// <summary>
        /// Logs the off.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogOff()
        {
            AuditLogReport auditLog = new AuditLogReport { RequestedUserName = GetLoggedInUserName(), Description = Constants.UserLoggedOut };

            //Save Logout Audit Log
            PostApiResponse<int>(Constants.User, Constants.SaveAuditLog, auditLog, true);

            RemoveSessionVariables();
            //FIXED-RAGINI-FEB16 - Why we are using this. Now Forms authentication is not used in app anymore this can be removed
            return RedirectToAction(Constants.Login, Constants.Account, new { area = "" });
        }

        //FIXED-RAGINI-FEB16 - This method should go in Base controller \ SessionController. Individual files should not manipulate Session directly

        /// <summary>
        /// Get the Name of the User Logged in
        /// </summary>
        [HttpGet]
        public JsonResult GetUserName()
        {
            if (Session[Constants.UserFacilitiesSessionString] != null)
            {
                UserInfo userInfo = Session[Constants.UserFacilitiesSessionString] as UserInfo;
                return (userInfo != null) ? Json(new { userInfo.UserName }, JsonRequestBehavior.AllowGet) : Json(new { UserName = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { UserName = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Downloads the user manual.
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadUserManual()
        {
            string userManualDocumentpath = Server.MapPath(GlobalConfigVariable.UserManualFilePath);
            string fileName = new FileInfo(userManualDocumentpath).Name;
            return File(userManualDocumentpath, MimeAssistant.GetMimeTypeByExtension(Enums.DownloadFileType.Pdf.ToString()), fileName);
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        private bool ValidateUser(string password, User userDetails)
        {
            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(userDetails.PasswordHash) &&
                !string.IsNullOrEmpty(userDetails.PasswordSalt))
            {
                string passwordInput = string.Format("{0}{1}", password, userDetails.PasswordSalt);

                string passwordStored = EncryptDecryptPassword.DecryptText(userDetails.PasswordHash);
                userDetails.ValidLogin = String.CompareOrdinal(passwordInput, passwordStored) == 0;
            }
            if (userDetails.UserTypeId != 1) //not SSI Admin
                PostApiResponse<bool>(Constants.User, Constants.UpdateUserLogin, userDetails, true);
            return userDetails.ValidLogin;
        }

        /// <summary>
        /// User Validation
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userDetails"></param>
        private bool UserValidation(string password, User userDetails)
        {
            string lastLoginDate = userDetails.CurrentDateTime;
            //validate user method here 
            if (ValidateUser(password, userDetails))
            {
                FormsAuthentication.SetAuthCookie(userDetails.UserName, true);
                //get user facility details
                User userDetail = PostApiResponse<User>(Constants.User,
                    Constants.GetUserFacilityDetails, userDetails, true);

                UserInfo userInfo = new UserInfo
                {
                    AssignedFacilities = new List<FacilityViewModel>(),
                    UserName = userDetails.UserName,
                    UserTypeId = userDetails.UserTypeId,
                    UserKey = userDetails.UserKey,
                    UserId = userDetails.UserId,
                    LastLoginDate =
                        (Convert.ToInt32(Enums.UserRoles.SsiAdmin) != userDetails.UserId)
                            ? (lastLoginDate != string.Empty) ? Utilities.GetLocalTimeString(_currentDatetime, Convert.ToDateTime(lastLoginDate)) : Utilities.GetLocalTimeString(_currentDatetime, DateTime.UtcNow)
                            : string.Empty,
                    PasswordExpirationDays = Convert.ToString(userDetails.PasswordExpirationDays),
                    LandingPageId = userDetails.LandingPageId
                };
                if (userInfo.UserTypeId != Convert.ToInt32(Enums.UserRoles.SsiAdmin))
                {
                    foreach (FacilityViewModel facilityViewModel in
                        userDetail.SelectedFacilityList.Select(facilityInfo => new FacilityViewModel
                        {
                            FacilityId = facilityInfo.FacilityId,
                            FacilityName = facilityInfo.FacilityName,
                            DataSource = facilityInfo.ConnectionString,
                            SsiNumber = facilityInfo.SsiNumber,
                            FacilityDisplayName = facilityInfo.DisplayName
                        }))
                    {
                        userInfo.AssignedFacilities.Add(facilityViewModel);
                    }
                }

                TempData[Constants.UserInfo] = userInfo;
                return true;
            }
            return false;
        }
        /// <summary>
        /// User Settings
        /// </summary>
        public ActionResult UserSetting()
        {
            User user = new User { UserId = GetUserInfo().UserId };
            byte landingPageId = PostApiResponse<byte>(Constants.User, Constants.GetUserLandingPage, user, true);
            UserViewModel userSetting = new UserViewModel { LandingPageId = landingPageId };
            return View(userSetting);
        }
        /// <summary>
        /// Save User Setting
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public ActionResult SaveUserLandingPage(User userModel)
        {
            bool isSuccess = false;
            if (userModel != null)
            {
                isSuccess = PostApiResponse<bool>(Constants.User, Constants.SaveUserLandingPage, userModel, true);
            }
            return Json(new { isSuccess });

        }
    }
}
