using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IUserRepository : IDisposable
    {

        /// <summary>
        /// Gets all User models.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        List<User> GetAllUserModels(User data);

        /// <summary>
        /// Gets the User By ID.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        User GetUserById(User data);

        /// <summary>
        /// Deletes the User by identifier.
        /// </summary>
        /// <param name="data">The User to delete.</param>
        /// <returns></returns>
        bool DeleteUser(User data);

        /// <summary>
        /// Adds the edit user basic information.
        /// </summary>
        /// <param name="userInfo">The user.</param>
        /// <returns></returns>
        User AddEditUser(User userInfo);

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        User GetUserDetails(User userInfo);

        /// <summary>
        /// Get User Facility Details
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        User GetUserFacilityDetails(User userInfo);

        /// <summary>
        /// Update user log on login
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        bool UpdateUserLogin(User userInfo);

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        User SaveUserAccountSetting(User userInfo);

        /// <summary>
        /// Save Email Log
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        bool SaveEmailLog(User userInfo);

        /// <summary>
        /// Validate Token
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        User ValidateToken(User userInfo);

        /// <summary>
        /// Save AuditLog for logOut
        /// </summary>
        /// <param name="auditLog"></param>
        /// <returns></returns>
        int SaveAuditLog(AuditLogReport auditLog);

        /// <summary>
        /// Get User Setting
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        byte GetUserLandingPage(int userId);

        /// <summary>
        /// Save User Setting 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        bool SaveUserLandingPage(User userInfo);
    }
}

