using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class UserLogic
    {
        /// <summary>
        /// Initializes repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLogic"/> class.
        /// </summary>
        public UserLogic()
        {
            _userRepository = Factory.CreateInstance<IUserRepository>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="userRepository"/> class.
        /// </summary>
        /// <param name="userRepository">The User details repository.</param>
        public UserLogic(IUserRepository userRepository)
        {
            if (userRepository != null)
            {
                _userRepository = userRepository;
            }
        }

        /// <summary>
        /// Gets all User models.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<User> GetAllUserModels(User data)
        {
            return _userRepository.GetAllUserModels(data);
        }

        /// <summary>
        /// Gets  User By Id
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public User GetUserById(User data)
        {
            return _userRepository.GetUserById(data);
        }

        /// <summary>
        /// Deletes the User.
        /// </summary>
        /// <param name="data">The data to delete.</param>
        /// <returns></returns>
        public bool DeleteUser(User data)
        {
            return _userRepository.DeleteUser(data);
        }

        /// <summary>
        /// Adds the edit User basic information.
        /// </summary>
        /// <param name="userInfo">The User.</param>
        /// <returns></returns>
        public User AddEditUser(User userInfo)
        {
            if (userInfo != null)
            {
                userInfo.SelectedFacility = (userInfo.SelectedFacilityList.Count > 0)
                    ? string.Join(Constants.Comma,
                        userInfo.SelectedFacilityList.Select(facility => facility.FacilityId).ToList())
                    : Constants.DefaultSelectedFacility;
                return _userRepository.AddEditUser(userInfo);
            }
            return null;
        }

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public User GetUserDetails(User userInfo)
        {
            return _userRepository.GetUserDetails(userInfo);
        }

        /// <summary>
        /// Get User Facility Details
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public User GetUserFacilityDetails(User userInfo)
        {
            return _userRepository.GetUserFacilityDetails(userInfo);
        }

        /// <summary>
        /// Update user log on login
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public bool UpdateUserLogin(User userInfo)
        {
            return _userRepository.UpdateUserLogin(userInfo);
        }

        /// <summary>
        /// Save User Account Setting
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public User SaveUserAccountSetting(User userInfo)
        {
            return _userRepository.SaveUserAccountSetting(userInfo);
        }

        /// <summary>
        /// SaveEmailLog
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool SaveEmailLog(User userInfo)
        {
            return _userRepository.SaveEmailLog(userInfo);
        }

        /// <summary>
        /// Validate Token
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public User ValidateToken(User userInfo)
        {
            return _userRepository.ValidateToken(userInfo);
        }

        /// <summary>
        /// Save AuditLog
        /// </summary>
        /// <param name="auditLog"></param>
        /// <returns></returns>
        public int SaveAuditLog(AuditLogReport auditLog)
        {
            return _userRepository.SaveAuditLog(auditLog);
        }

        /// <summary>
        /// Get User Setting
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public byte GetUserLandingPage(int userId)
        {
            return _userRepository.GetUserLandingPage(userId);
        }

        /// <summary>
        /// Save User Setting
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool SaveUserLandingPage(User userInfo)
        {
            return _userRepository.SaveUserLandingPage(userInfo);
        }
    }
}
