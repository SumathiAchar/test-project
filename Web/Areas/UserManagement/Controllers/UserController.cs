using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Kendo.DynamicLinq;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;

//FIXED-FEB16 - Remove directives which are not required
namespace SSI.ContractManagement.Web.Areas.UserManagement.Controllers
{
    public class UserController : CommonController
    {
        public ActionResult Index(int facilityId)
        {
            UserViewModel user = new UserViewModel { FacilityId = facilityId };
            return View(user);
        }

        /// <summary>
        /// Add User 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddUser(UserViewModel userInfo)
        {
            User facilityFullInfo = PostApiResponse<User>(Constants.User, Constants.GetUserById, userInfo, true);

            UserViewModel facilityInfo = Mapper.Map<User, UserViewModel>(facilityFullInfo);

            facilityInfo.AvailableFacilityList = (from availableFacilityList in facilityInfo.AvailableFacilityList
                                                  where !(facilityInfo.SelectedFacilityList.Select(facility => facility.FacilityId).ToList()).Contains(availableFacilityList.FacilityId)
                                                  select availableFacilityList).OrderBy(q => q.FacilityName).ToList();

            facilityInfo.SelectedFacilityList = facilityInfo.SelectedFacilityList.OrderBy(q => q.FacilityName).ToList();
            return View(facilityInfo);
        }

        /// <summary>
        /// Get All User List
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllUsers(int take, int skip, int facilityId, int userId)
        {
            PageSetting pageSetting = new PageSetting { Skip = skip, Take = take };
            User userInfo = new User { FacilityId = facilityId, PageSetting = pageSetting, UserId = userId };
            DataSourceResult userInfoResult = new DataSourceResult();
            List<User> userModels = PostApiResponse<List<User>>(Constants.User, Constants.GetAllUserModels, userInfo, true);
            if (userModels != null)
            {
                userInfoResult.Data = Mapper.Map<List<User>, List<UserViewModel>>(userModels);
                userInfoResult.Total = userModels.Select(a => a.TotalRecords).FirstOrDefault();
            }
            return Json(userInfoResult);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            User user = new User { UserId = id, RequestedUserName = GetLoggedInUserName() };
            bool isSuccess = PostApiResponse<bool>(Constants.User, Constants.Delete, user, true);
            //Fixed-RAGINI-FEB16 - Correct spelling mistakes
            return Json(new { isSuccess });
        }

        /// <summary>
        /// Insert / Update User
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public JsonResult AddEditUser(UserViewModel userInfo)
        {
            userInfo.RequestedUserName = GetLoggedInUserName();
            User user = Mapper.Map<UserViewModel, User>(userInfo);
            if (user.UserId == Constants.DefaultUserId)
            {
                user.UserGuid = Guid.NewGuid();
            }
            user.UserKey = (user.UserId == Constants.DefaultUserId) ? Guid.NewGuid().ToString() : Guid.Empty.ToString();
            user.UserTypeId =
                Convert.ToInt32(userInfo.IsAdmin ? Enums.UserRoles.CmAdmin : Enums.UserRoles.CmUser);
            User userData = PostApiResponse<User>(Constants.User, Constants.AddEditUser, user, true);
            //Fixed-RAGINI-FEB2016 - what are these magic integers -1, -2, you can create enum or constants and use it
            if (userData != null && (userData.UserId != Constants.UserNameExist) && userData.UserGuid != Guid.Empty &&
                userData.UserId != Constants.InValidDomainName)
            {
                userData.EmailType = Convert.ToInt32(Enums.EmailType.AccountActivation);
                IsEmailNotificationSend(userData);
            }
            return Json(userData);
        }

        /// <summary>
        /// Inserting userInfoData into DataBase
        /// </summary>
        /// <param name="userInfo" />
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveUserAccountSetting(UserViewModel userInfo)
        {
            userInfo.RequestedUserName = GetLoggedInUserName();
            User user = Mapper.Map<UserViewModel, User>(userInfo);
            user.UserGuid = Guid.NewGuid();
            bool isMailSend = false;
            User userDetail = PostApiResponse<User>(Constants.User, Constants.SaveUserAccountSetting, user, true);
            if (userDetail != null)
            {
                isMailSend = IsEmailNotificationSend(userDetail);
            }
            return Json(isMailSend);
        }

        /// <summary>
        /// IsEmail Notification Send
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsEmailNotificationSend(User user)
        {
            if (user != null)
            {
                string emailContent, emailSubject = string.Empty;
                string serverMapPath = user.EmailType == Convert.ToInt32(Enums.EmailType.AccountActivation)
                    ? Server.MapPath(Constants.MailMessageTemplate)
                    : Server.MapPath(Constants.PasswordResetTemplate);
                using (StreamReader reader = new StreamReader(serverMapPath))
                {
                    emailContent = reader.ReadToEnd();
                }
                switch ((Enums.EmailType)Convert.ToInt32(user.EmailType))
                {
                    case Enums.EmailType.AccountActivation:
                        emailSubject = Constants.MailSubject;
                        emailContent = emailContent.Replace("##LINK", Constants.ActivationMailLink);
                        break;
                    case Enums.EmailType.AccountReset:
                        emailSubject = Constants.MailSubjectAccountReset;
                        emailContent = emailContent.Replace("##LINK", Constants.ResetAccountMailLink);
                        break;
                    case Enums.EmailType.ChangePassword:
                        emailSubject = Constants.MailSubjectChangePassword;
                        emailContent = emailContent.Replace("##LINK", Constants.ChangePasswordMailLink);
                        break;
                    case Enums.EmailType.PasswordReset:
                        emailSubject = Constants.MailSubjectPasswordReset;
                        emailContent = emailContent.Replace("##LINK", Constants.ResetPasswordMailLink);
                        break;
                    case Enums.EmailType.RecoverPassword:
                        emailSubject = Constants.MailSubjectRecoverPassword;
                        emailContent = emailContent.Replace("##LINK", Constants.RecoverPasswordMailLink);
                        break;
                }

                Uri url = System.Web.HttpContext.Current.Request.Url;
                string domainName = string.Format("{0}/{1}", url.GetLeftPart(UriPartial.Authority),
                    (url.Segments.Length >= 2) ? url.Segments[1] : String.Empty);

                //email token
                string emailLink = string.Format("{0}{1}{2}", domainName, Constants.EmailUrl,
                  Server.UrlEncode(EncryptDecryptPassword.EncryptText(user.UserGuid.ToString())));

                emailContent = emailContent.Replace("##DATA", user.EmailType == Convert.ToInt32(Enums.EmailType.AccountActivation) ?
                    Constants.ActivationMailBody : Constants.PasswordMailBody);

                // Change http to https in email link url for production not other environment
                string httpsEmailLink = emailLink;
                if (!Constants.HttpUrls.Contains(url.Host))
                {
                    httpsEmailLink = emailLink.Replace(Constants.Http, Constants.Https);
                }
                emailContent = emailContent.Replace("##GUID", httpsEmailLink);

                if (user.EmailType != Convert.ToInt32(Enums.EmailType.AccountActivation))
                {
                    emailContent = emailContent.Replace("##NAME", string.Format("{0} {1}", user.FirstName, user.LastName));
                }

                try
                {
                    SaveEmailLog(user);
                    Utilities.SendMail(emailSubject, emailContent, user.UserName);
                }
                catch (Exception ex)
                {
                    Log.LogError("Send Email Exeception", user.UserName, ex);
                }
            }
            return true;
        }

        /// <summary>
        /// Save Email Log
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private void SaveEmailLog(User user)
        {
            user.RequestedUserName = GetLoggedInUserName();
            PostApiResponse<bool>(Constants.User, Constants.SaveEmailLog, user, true);
        }
    }
}
