using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;

namespace SSI.ContractManagement.Web.Areas.Common.Controllers
{
    public class BaseController : Controller
    {
        //FIXED-RAGINI-FEB16 - Initialize this in constructor only
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        protected BaseController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(Convert.ToString(GlobalConfigVariable.WebApiurl)),
                Timeout = TimeSpan.FromMilliseconds(Convert.ToInt32(GlobalConfigVariable.HttpClientTimeOut)),
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Gets the API response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiController">The API controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="isUserManagement">Is from user management.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        protected T GetApiResponse<T>(string apiController, string action, long? id, bool isUserManagement = false)
        {
            //FIXED-RAGINI-FEB16 - Use String.format
            string requestUri = string.Format("{0}{1}{2}{3}{4}{5}", "api/", apiController, "/", action,"/",id);

            //update request header
            if (!isUserManagement)
                UpdateRequestHeader();

            //Call webapi GET method using HttpClient
            HttpResponseMessage response = _client.GetAsync(requestUri).Result;
            if (response.IsSuccessStatusCode)
                return (T)response.Content.ReadAsAsync(typeof(T)).Result;
            //REVIEW-RAGINI-FEB16 - Why Exception is thrown simply. Add the error response received from GetAsync call
            // to give more specific details to User or for debugging what went wrong
            //return Exception if something went wrong
            throw new InvalidProgramException();
        }

        /// <summary>
        /// Gets the API response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiController">The API controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="isUserManagement">Is from user management.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        protected T GetApiResponse<T>(string apiController, string action, bool isUserManagement = false)
        {
            //FIXED-RAGINI-FEB16 - Use String.format
            string requestUri = string.Format("{0}{1}{2}{3}", "api/", apiController, "/", action);

            //update request header 
            if (!isUserManagement)
                UpdateRequestHeader();

            //Call webapi GET method using HttpClient
            HttpResponseMessage response = _client.GetAsync(requestUri).Result;
            if (response.IsSuccessStatusCode)
                return (T)response.Content.ReadAsAsync(typeof(T)).Result;

            //REVIEW-RAGINI-FEB16 - Why Exception is thrown simply. Add the error response received from GetAsync call
            // to give more specific details to User or for debugging what went wrong
            //return Exception if something went wrong
            throw new InvalidProgramException();
        }

        /// <summary>
        /// Posts the API response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiController">The API controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="data">The data.</param>
        /// <param name="isUserManagement">Is from user management.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        protected T PostApiResponse<T>(string apiController, string action, object data, bool isUserManagement = false)
        {
            string requestUri = string.Format("{0}{1}{2}{3}", "api/", apiController, "/", action);

            //update request header 
            if (!isUserManagement)
                UpdateRequestHeader();

            //Call webapi POST method using HttpClient
            var messageData = _client.PostAsJsonAsync(requestUri, data);
            HttpResponseMessage response = null;
            if (!(messageData != null && messageData.Status == System.Threading.Tasks.TaskStatus.Faulted))
                if (messageData != null)
                {
                    response = messageData.Result;
                }

            if (response != null)
                if (response.IsSuccessStatusCode)
                    return (T)response.Content.ReadAsAsync(typeof(T)).Result;

            //REVIEW-RAGINI-FEB16 - Why Exception is thrown simply. Add the error response received from GetAsync call
            // to give more specific details to User or for debugging what went wrong

            //return Exception if something went wrong
            throw new InvalidProgramException();
        }

        /// <summary>
        /// Updates the request header.
        /// </summary>
        private void UpdateRequestHeader()
        {
            //FIXED-RAGINI-FEB16 - Check if header contains value then remove it. 
            if (_client.DefaultRequestHeaders.Contains(Constants.BubbleDataSource))
            {
                _client.DefaultRequestHeaders.Remove(Constants.BubbleDataSource);
            }
            _client.DefaultRequestHeaders.Add(Constants.BubbleDataSource, GetCurrentFacilityId().ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Called when an unhandled exception occurs in the action.
        /// </summary>
        /// <param name="exceptionContext">Information about exception.</param>
        protected override void OnException(ExceptionContext exceptionContext)
        {
            Log.LogError("Web Exception : ", Convert.ToString(GetCurrentUserName()), exceptionContext.Exception);
        }

        /// <summary>
        /// Gets the assigned facilities.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FacilityViewModel> GetAssignedFacilities()
        {
            if (Session[Constants.UserFacilitiesSessionString] != null)
            {
                UserInfo userInfoViewModel = Session[Constants.UserFacilitiesSessionString] as UserInfo;
                return (userInfoViewModel != null) ? userInfoViewModel.AssignedFacilities : null;
            }
            //SSI Error page should be shown after logging the error details
            throw new InvalidProgramException();
        }

        /// <summary>
        /// Gets the logged in username.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserName()
        {
            UserInfo userInfo = Session[Constants.UserFacilitiesSessionString] as UserInfo;
            if (userInfo != null) return userInfo.UserName;
            throw new InvalidProgramException();
        }

        /// <summary>
        /// Gets the name of the logged in user.
        /// </summary>
        /// <returns></returns>
        public string GetLoggedInUserName()
        {
            UserInfo userInfo = Session[Constants.UserFacilitiesSessionString] as UserInfo;
            return userInfo != null ? userInfo.UserName : null;
        }

        /// <summary>
        /// Gets the user information from session.
        /// </summary>
        /// <returns></returns>
        public UserInfo GetUserInfo()
        {
            if (Session[Constants.UserFacilitiesSessionString] != null)
                return (UserInfo)Session[Constants.UserFacilitiesSessionString];
            return new UserInfo();
        }

        /// <summary>
        /// Gets the user facility ids.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetUserFacilityIds()
        {
            List<int> facilityIds = new List<int>();
            if (Session[Constants.UserFacilitiesSessionString] != null)
            {
                UserInfo userInfoViewModel = Session[Constants.UserFacilitiesSessionString] as UserInfo;
                if (userInfoViewModel != null && userInfoViewModel.AssignedFacilities != null &&
                    userInfoViewModel.AssignedFacilities.Any())
                {
                    facilityIds = userInfoViewModel.AssignedFacilities.Select(f => f.FacilityId).ToList();
                }
                return facilityIds;
            }
            return facilityIds;
        }

        /// <summary>
        /// Gets the user Guid identifier.
        /// </summary>
        /// <returns></returns>
        public string GetUserKey()
        {
            UserInfo userInfoViewModel = Session[Constants.UserFacilitiesSessionString] as UserInfo;
            return userInfoViewModel != null ? userInfoViewModel.UserKey : null;
        }

        /// <summary>
        /// Gets the ssi number based on facility identifier.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        public List<int> GetSsiNumberBasedOnFacilityId(long? facilityId)
        {
            UserInfo userInfo = new UserInfo();
            if (Session[Constants.UserFacilitiesSessionString] != null)
            {
                userInfo = Session[Constants.UserFacilitiesSessionString] as UserInfo;
            }
            if (userInfo != null && userInfo.AssignedFacilities != null && userInfo.AssignedFacilities.Count > 0)
            {
                if (userInfo.AssignedFacilities.Any(q => facilityId.HasValue && q.FacilityId == facilityId.Value))
                {
                    return userInfo.AssignedFacilities.Where(q => facilityId.HasValue && q.FacilityId == facilityId.Value).SelectMany(a => a.SsiNumber).ToList();
                }
            }
            return new List<int>();
        }

        /// <summary>
        /// Gets the current facility identifier.
        /// </summary>
        /// <returns></returns>
        public int GetCurrentFacilityId()
        {
            if (Session[Constants.CurrentFacilityIdSessionString] != null)
                return (int)Session[Constants.CurrentFacilityIdSessionString];
            return 0;
        }

        /// <summary>
        /// Gets the current facility identifier.
        /// </summary>
        /// <returns></returns>
        public int GetUserTypeId()
        {
            UserInfo userInfo = Session[Constants.UserFacilitiesSessionString] as UserInfo;
            return (userInfo != null) ? userInfo.UserTypeId : 0;
        }

        /// <summary>
        /// Gets the name of the current facility.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentFacilityName()
        {
            long currentFacilityId = GetCurrentFacilityId();
            FacilityViewModel currentFacility =
                GetAssignedFacilities().FirstOrDefault(q => q.FacilityId == currentFacilityId);
            if (currentFacility != null)
                return currentFacility.FacilityDisplayName;
            return string.Empty;
        }

        /// <summary>
        /// Removes the session variables.
        /// </summary>
        public void RemoveSessionVariables()
        {
            Session[Constants.IsUserLoggedIn] = null;
            Session[Constants.UserFacilitiesSessionString] = null;
            Session[Constants.CurrentFacilityIdSessionString] = null;
            Session[Constants.LastRequestedNodeSessionString] = null;
            Session[Constants.LastHighlightedNodeIdSessionString] = null;
            Session[Constants.LastExpandedNodeIdSessionString] = null;
        }

        /// <summary>
        /// Gets the auto refresh information.
        /// </summary>
        public bool GetAutoRefresh()
        {
            if (Session[Constants.IsAutoRefresh] != null)
                return Convert.ToBoolean(Session[Constants.IsAutoRefresh]);
            return false;
        }
    }
}
