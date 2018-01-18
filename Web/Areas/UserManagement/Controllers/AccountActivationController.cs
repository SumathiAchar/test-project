
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;

//FIXED-FEB16 - Remove directives which are not required

namespace SSI.ContractManagement.Web.Areas.UserManagement.Controllers
{
    public class AccountActivationController : BaseController
    {
        /// <summary>
        /// View AccountActivation Page
        /// </summary>
        public ActionResult AccountActivation(string token)
        {
            string guid = EncryptDecryptPassword.DecryptText(token);
            if (!string.IsNullOrEmpty(guid))
            {
                UserViewModel userViewModel = new UserViewModel { UserGuid = new Guid(guid) };
                User user = Mapper.Map<UserViewModel, User>(userViewModel);
                User userinfo = PostApiResponse<User>(Constants.User, Constants.ValidateToken, user, true);
                if (userinfo != null)
                {
                    userViewModel = Mapper.Map<User, UserViewModel>(userinfo);
                    return View(userViewModel);
                }
            }
            return View(new UserViewModel());
        }

        /// <summary>
        /// View SecurityQuestion Page
        /// </summary>
        public ActionResult SecurityQuestionsAndPassword()
        {
            return View();
        }

        /// <summary>
        /// Validate email exist or not.
        /// </summary>
        [HttpPost]
        public JsonResult IsUserEmailExist(UserViewModel userViewModel)
        {
            User user = Mapper.Map<UserViewModel, User>(userViewModel);
            var isEmailExist = PostApiResponse<int>(Constants.AccountActivation, Constants.IsUserEmailExist, user, true);
            return Json(isEmailExist);
        }

        /// <summary>
        /// Validate email exist or not and locked or not.
        /// </summary>
        [HttpPost]
        public JsonResult IsUserEmailLockedOrNot(UserViewModel userViewModel)
        {
            User user = Mapper.Map<UserViewModel, User>(userViewModel);
            var isEmailExist = PostApiResponse<int>(Constants.AccountActivation, Constants.IsUserEmailLockedOrNot, user, true);
            return Json(isEmailExist);
        }


        /// <summary>
        /// Get list of question.
        /// </summary>
        [HttpPost]
        public JsonResult GetSecurityQuestion(int userId)
        {
            User userinfo = new User { UserId = userId };
            List<SecurityQuestion> securityQuestions = PostApiResponse<List<SecurityQuestion>>(Constants.AccountActivation, Constants.GetSecurityQuestion, userinfo, true);
            List<SecurityQuestionViewModel> securityQuestionsViewModel = Mapper.Map<List<SecurityQuestion>, List<SecurityQuestionViewModel>>(securityQuestions);
            return Json(new { tableNames = securityQuestionsViewModel });
        }

        /// <summary>
        /// Add question answer and password.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddQuestionAnswerAndPassword(UserViewModel userView)
        {
            foreach (var answer in userView.UserSecurityQuestion)
            {
                userView.UserSecurityQuestion[userView.UserSecurityQuestion.FindIndex(a => a.Equals(answer))].Answer = EncryptAnswer(answer.Answer);
            }
            User user = Mapper.Map<UserViewModel, User>(userView);
            int securityQuestionData = PostApiResponse<int>(Constants.AccountActivation, Constants.AddQuestionAnswerAndPassword, user, true);
            return Json(securityQuestionData);
        }

        /// <summary>
        /// Encrypt Answer
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        // FIXED-FEB16 - Move "[^0-9a-z]+" to constant 
        public string EncryptAnswer(string answer)
        {
            string encryptedString = string.Empty;
            if (answer != null)
            {
                string convertToLower = answer.ToLower();
                encryptedString = Regex.Replace(convertToLower, Constants.SpecialCharacterPatterne, string.Empty);
            }
            return EncryptDecryptPassword.EncryptText(encryptedString);
        }

        /// <summary>
        /// Inserting userInfoData into DataBase
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public JsonResult SaveUserAccountSetting(UserViewModel userInfo)
        {
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
            user.RequestedUserName = user.UserName;
            PostApiResponse<bool>(Constants.User, Constants.SaveEmailLog, user, true);
        }
    }
}
