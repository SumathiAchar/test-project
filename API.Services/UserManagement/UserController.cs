using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.UserManagement
{
    public class UserController : ApiController
    {
        readonly UserLogic _userLogic;

        public UserController()
        {
            _userLogic = new UserLogic();
        }

        /// <summary>
        /// Gets the User full information.
        /// </summary>
        /// <param name="data">The User information.</param>
        /// <returns></returns>
        [HttpPost]
        public List<User> GetAllUserModels(User data)
        {
            return _userLogic.GetAllUserModels(data);
        }

        /// <summary>
        /// Get User By ID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public User GetUserById(User data)
        {
            return _userLogic.GetUserById(data);
        }

        /// <summary>
        /// Deletes the User.
        /// </summary>
        /// <param name="data">The data to delete.</param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteUser(User data)
        {
            return _userLogic.DeleteUser(data);
        }

        /// <summary>
        /// Add/edit User basic information.
        /// </summary>
        /// <param name="userInfo">The User.</param>
        /// <returns></returns>
        public User AddEditUser(User userInfo)
        {
            return _userLogic.AddEditUser(userInfo);
        }

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public User GetUserDetails(User userInfo)
        {
            return _userLogic.GetUserDetails(userInfo);
        }

        /// <summary>
        /// Get User Facility Details
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public User GetUserFacilityDetails(User userInfo)
        {
            return _userLogic.GetUserFacilityDetails(userInfo);
        }

        /// <summary>
        /// Update user log on login
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool UpdateUserLogin(User userInfo)
        {
            return _userLogic.UpdateUserLogin(userInfo);
        }

        /// <summary>
        /// Save User Account Setting
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public User SaveUserAccountSetting(User userInfo)
        {
            return _userLogic.SaveUserAccountSetting(userInfo);
        }

        /// <summary>
        /// SaveEmailLog
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public bool SaveEmailLog(User userInfo)
        {
            return _userLogic.SaveEmailLog(userInfo);
        }

        /// <summary>
        /// ValidateToken
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public User ValidateToken(User userInfo)
        {
            return _userLogic.ValidateToken(userInfo);
        }
        /// <summary>
        /// Save AuditLog
        /// </summary>
        /// <param name="auditLog"></param>
        /// <returns></returns>
        [HttpPost]
        public int SaveAuditLog(AuditLogReport auditLog)
        {
            return _userLogic.SaveAuditLog(auditLog);
        }

        /// <summary>
        /// Get User Setting
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public byte GetUserLandingPage(User userInfo)
        {
            return _userLogic.GetUserLandingPage(userInfo.UserId);
        }

        /// <summary>
        /// Save User Setting
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public bool SaveUserLandingPage(User userInfo)
        {
            return _userLogic.SaveUserLandingPage(userInfo);
        }
    }
}
