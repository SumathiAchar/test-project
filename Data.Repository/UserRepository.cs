using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class UserRepository : BaseRepository, IUserRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        public UserRepository()
        {
            // Create a database object
            // Specify the name of database
            _db = DatabaseFactory.CreateDatabase("CMMembershipConnectionString");
        }

        /// <summary>
        /// Gets all user Informations Based on Selected FacilityId.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public List<User> GetAllUserModels(User data)
        {
            _cmd = _db.GetStoredProcCommand("GetAllUser");
            _db.AddInParameter(_cmd, "@LoggedInUserId", DbType.Int64, data.UserId);
            _db.AddInParameter(_cmd, "@FacilityID", DbType.Int64, data.FacilityId);
            _db.AddInParameter(_cmd, "@Take", DbType.Int32, data.PageSetting.Take);
            _db.AddInParameter(_cmd, "@Skip", DbType.Int32, data.PageSetting.Skip);
            DataSet userModelDataSet = _db.ExecuteDataSet(_cmd);

            var userModelsList = new List<User>();
            if (userModelDataSet.Tables[0].Rows.Count > 0)
            {
                DataRow[] dataRows = userModelDataSet.Tables[0].Select();
                userModelsList.AddRange(dataRows.Select(userRow => new User
                {
                    UserId = Convert.ToInt32(userRow["UserId"]),
                    FacilityId = Convert.ToInt32(userRow["FacilityId"]),
                    FirstName = Convert.ToString(userRow["FirstName"]),
                    LastName = Convert.ToString(userRow["LastName"]),
                    MiddleName = Convert.ToString(userRow["MiddleName"]),
                    UserName = Convert.ToString(userRow["UserName"]),
                    IsLocked = Convert.ToBoolean(userRow["IsLocked"]),
                    IsAdmin = Convert.ToBoolean(userRow["IsAdmin"]),
                    TotalRecords = Convert.ToInt32(userModelDataSet.Tables[1].Rows[0][0])
                }));
            }
            return userModelsList;
        }

        /// <summary>
        /// Gets user by id .
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public User GetUserById(User data)
        {
            _cmd = _db.GetStoredProcCommand("GetUserById");
            _db.AddInParameter(_cmd, "@UserId", DbType.Int64, data.UserId);
            _db.AddInParameter(_cmd, "@LoginUserId", DbType.Int64, data.LoggedInUserId);
            DataSet userModelDataSet = _db.ExecuteDataSet(_cmd);

            var userModelsList = new User();
            if (userModelDataSet.Tables.Count > 1)
            {
                if (userModelDataSet.Tables[0].Rows.Count > 0)
                {
                    userModelsList.AvailableFacilityList = new List<Facility>();
                    DataRow[] dataRows = userModelDataSet.Tables[0].Select();
                    userModelsList.AvailableFacilityList.AddRange(dataRows.Select(facilityRow => new Facility
                    {
                        FacilityId = Convert.ToInt32(facilityRow["FacilityId"]),
                        FacilityName = Convert.ToString(facilityRow["FacilityName"])

                    }));
                }
                if (userModelDataSet.Tables[1].Rows.Count > 0)
                {
                    userModelsList.SelectedFacilityList = new List<Facility>();
                    DataRow[] dataRows = userModelDataSet.Tables[1].Select();
                    userModelsList.SelectedFacilityList.AddRange(dataRows.Select(facilityRow => new Facility
                    {
                        FacilityId = Convert.ToInt32(facilityRow["FacilityId"]),
                        FacilityName = Convert.ToString(facilityRow["FacilityName"])

                    }));
                }
                if (userModelDataSet.Tables[2].Rows.Count > 0)
                {
                    userModelsList.UserId = Convert.ToInt32(userModelDataSet.Tables[2].Rows[0]["UserId"]);
                    userModelsList.FirstName = Convert.ToString(userModelDataSet.Tables[2].Rows[0]["FirstName"]);
                    userModelsList.LastName = Convert.ToString(userModelDataSet.Tables[2].Rows[0]["LastName"]);
                    userModelsList.MiddleName = Convert.ToString(userModelDataSet.Tables[2].Rows[0]["MiddleName"]);
                    userModelsList.UserName = Convert.ToString(userModelDataSet.Tables[2].Rows[0]["UserName"]);
                    userModelsList.IsLocked = Convert.ToBoolean(userModelDataSet.Tables[2].Rows[0]["IsLocked"]);
                    userModelsList.IsAdmin = Convert.ToBoolean(userModelDataSet.Tables[2].Rows[0]["IsAdmin"]);
                    userModelsList.IsPasswordResetLock = Convert.ToBoolean(userModelDataSet.Tables[2].Rows[0]["LockPasswordReset"]);
                }
            }
            return userModelsList;
        }

        /// <summary>
        /// Deletes the User by UserId.
        /// </summary>
        /// <param name="userInfo">The User to delete.</param>
        /// <returns></returns>
        public bool DeleteUser(User userInfo)
        {
            //holds the response data
            bool returnvalue = false;
            if (userInfo != null)
            {
                _cmd = _db.GetStoredProcCommand("DeleteUserById");
                _db.AddInParameter(_cmd, "@UserId", DbType.Int64, userInfo.UserId);
                _db.AddInParameter(_cmd, "@LoggedInUserName", DbType.String, userInfo.RequestedUserName.ToTrim());

                int updatedRow = _db.ExecuteNonQuery(_cmd);
                //returns response to Business layer
                if (updatedRow == 0)
                    returnvalue = true;
                return returnvalue;
            }
            return false;
        }

        /// <summary>
        /// Adds the edit user basic information.
        /// </summary>
        /// <param name="userInfo">The user.</param>
        /// <returns></returns>
        public User AddEditUser(User userInfo)
        {
            //Checks if input userInfo is not null
            if (userInfo != null)
            {
                //a separate SP to add USer history can be craeted and used whereever required.
                _cmd = _db.GetStoredProcCommand("AddEditUser");
                _db.AddInParameter(_cmd, "@UserId", DbType.Int32, userInfo.UserId);
                _db.AddInParameter(_cmd, "@UserGUID", DbType.Guid, userInfo.UserGuid);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, userInfo.UserName.ToTrim());
                _db.AddInParameter(_cmd, "@FirstName", DbType.String, userInfo.FirstName);
                _db.AddInParameter(_cmd, "@LastName", DbType.String, userInfo.LastName.ToTrim());
                _db.AddInParameter(_cmd, "@MiddleName", DbType.String, userInfo.MiddleName.ToTrim());
                _db.AddInParameter(_cmd, "@IsLocked", DbType.Boolean, userInfo.IsLocked);
                _db.AddInParameter(_cmd, "@UserTypeId", DbType.Int32, userInfo.UserTypeId);
                _db.AddInParameter(_cmd, "@SelectedFacility", DbType.String, userInfo.SelectedFacility);
                _db.AddInParameter(_cmd, "@UserKey", DbType.Guid, new Guid(userInfo.UserKey));
                _db.AddInParameter(_cmd, "@LoggedInUserName", DbType.String, userInfo.RequestedUserName.ToTrim());
                DataSet userModelDataSet = _db.ExecuteDataSet(_cmd);
                //Check if output result tables exists or not
                if (userModelDataSet.IsTableDataPopulated() && userModelDataSet.Tables[0].Rows[0]["UserId"] != DBNull.Value)
                {
                    userInfo.UserId = Convert.ToInt32(userModelDataSet.Tables[0].Rows[0]["UserId"]);
                    userInfo.UserGuid = new Guid(Convert.ToString(userModelDataSet.Tables[0].Rows[0]["UserGuid"]));
                }
            }
            return userInfo;
        }

        /// <summary>
        /// Get Logged in User Details
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public User GetUserDetails(User userInfo)
        {
            User userDetails = new User();
            _cmd = _db.GetStoredProcCommand("GetUserDetails");
            _db.AddInParameter(_cmd, "@UserName", DbType.String, userInfo.Email);
            DataSet userDataSet = _db.ExecuteDataSet(_cmd);
            if (userDataSet != null && userDataSet.Tables[0].Rows.Count > 0 && userDataSet.IsTableDataPopulated())
            {
                userDetails.UserId = Convert.ToInt32(userDataSet.Tables[0].Rows[0]["UserId"]);
                userDetails.UserGuid = new Guid(userDataSet.Tables[0].Rows[0]["UserGUID"].ToString());
                userDetails.UserName = userDataSet.Tables[0].Rows[0]["UserName"].ToString();
                userDetails.PasswordHash = userDataSet.Tables[0].Rows[0]["PasswordHash"].ToString();
                userDetails.PasswordSalt = userDataSet.Tables[0].Rows[0]["PasswordSalt"].ToString();
                userDetails.FirstName = userDataSet.Tables[0].Rows[0]["FirstName"].ToString();
                userDetails.LastName = userDataSet.Tables[0].Rows[0]["LastName"].ToString();
                userDetails.MiddleName = userDataSet.Tables[0].Rows[0]["MiddleName"].ToString();
                userDetails.IsLocked = Convert.ToBoolean(userDataSet.Tables[0].Rows[0]["IsLocked"]);
                userDetails.UserTypeId = Convert.ToInt32(userDataSet.Tables[0].Rows[0]["UserTypeId"]);
                userDetails.UserKey = userDataSet.Tables[0].Rows[0]["UserKey"].ToString();
                userDetails.LandingPageId = Convert.ToByte(userDataSet.Tables[0].Rows[0]["LandingPageID"]);
                if (userDetails.UserTypeId != Convert.ToInt32(Enums.UserRoles.SsiAdmin) && userDataSet.Tables[1].Rows.Count > 0)
                {
                    userDetails.IsPasswordExpired = Convert.ToBoolean(userDataSet.Tables[0].Rows[0]["IsPasswordExpired"]);
                    userDetails.PasswordExpirationDays = Convert.ToInt32(userDataSet.Tables[1].Rows[0]["DaysToExpiry"]);
                    userDetails.CurrentDateTime = Convert.ToString(userDataSet.Tables[0].Rows[0]["LastLoginDate"]);
                }
            }
            return userDetails;
        }

        /// <summary> 
        /// Get User Facility Details
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public User GetUserFacilityDetails(User userInfo)
        {
            //inner join  can be used and that will improve the performance also.
            List<Facility> facilities = new List<Facility>();
            _cmd = _db.GetStoredProcCommand("GetUserFacilityDetails");
            _db.AddInParameter(_cmd, "@UserId", DbType.Int32, userInfo.UserId);
            DataSet userFacilityDataSet = _db.ExecuteDataSet(_cmd);
            if (userFacilityDataSet != null && userFacilityDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drow in userFacilityDataSet.Tables[0].Rows)
                {
                    var facility = new Facility
                    {
                        FacilityId = Convert.ToInt32(drow["FacilityId"]),
                        FacilityName = drow["FacilityName"].ToString(),
                        ConnectionString = drow["ConnectionString"].ToString(),
                        DisplayName = drow["DisplayName"].ToString()
                    };
                    if (userFacilityDataSet.Tables[1].Rows.Count > 0)
                    {
                        DataRow[] drSsiNumber = userFacilityDataSet.Tables[1].Select("FacilityId = " + Convert.ToInt32(drow["FacilityId"]));

                        if (drSsiNumber.Length > 0)
                        {
                            facility.SsiNumber = new List<int>();
                            foreach (var ssi in drSsiNumber)
                            {
                                facility.SsiNumber.Add(Convert.ToInt32(ssi[2]));
                            }
                        }
                        facilities.Add(facility);
                    }
                }
                userInfo.SelectedFacilityList = facilities;
            }
            return userInfo;
        }

        /// <summary>
        /// Save User AccountSetting
        /// </summary>
        /// <param name="userInfo">The User to delete.</param>
        /// <returns></returns>
        public User SaveUserAccountSetting(User userInfo)
        {
            if (userInfo != null)
            {
                _cmd = _db.GetStoredProcCommand("SaveUserAccountSetting");
                _db.AddInParameter(_cmd, "@UserId", DbType.Int64, userInfo.UserId);
                _db.AddInParameter(_cmd, "@UserGUID", DbType.Guid, userInfo.UserGuid);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, userInfo.RequestedUserName);
                DataSet userDetailDataSet = _db.ExecuteDataSet(_cmd);
                if (userDetailDataSet.IsTableDataPopulated())
                {
                    userInfo.FirstName = Convert.ToString(userDetailDataSet.Tables[0].Rows[0]["FirstName"]);
                    userInfo.LastName = Convert.ToString(userDetailDataSet.Tables[0].Rows[0]["LastName"]);
                    userInfo.UserName = Convert.ToString(userDetailDataSet.Tables[0].Rows[0]["UserName"]);
                }
            }
            return userInfo;
        }

        /// <summary>
        /// Save EmailLog
        /// </summary>
        /// <param name="userInfo">updating emaillog and user table.</param>
        /// <returns></returns>
        public bool SaveEmailLog(User userInfo)
        {
            if (userInfo != null)
            {
                _cmd = _db.GetStoredProcCommand("SaveEmailLog");
                _db.AddInParameter(_cmd, "@UserId", DbType.Int32, userInfo.UserId);
                _db.AddInParameter(_cmd, "@UserGUID", DbType.Guid, userInfo.UserGuid);
                _db.AddInParameter(_cmd, "@EmailType", DbType.Int32, userInfo.EmailType);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, userInfo.RequestedUserName);

                return Convert.ToBoolean(_db.ExecuteScalar(_cmd));
            }
            return false;
        }

        /// <summary>
        /// ValidateToken
        /// </summary>
        /// <param name="userInfo">ValidateToken.</param>
        /// <returns></returns>
        public User ValidateToken(User userInfo)
        {
            if (userInfo != null)
            {
                _cmd = _db.GetStoredProcCommand("ValidateToken");

                _db.AddInParameter(_cmd, "@UserGUID", DbType.Guid, userInfo.UserGuid);

                DataSet userDetailDataSet = _db.ExecuteDataSet(_cmd);
                if (userDetailDataSet.IsTableDataPopulated())
                {
                    userInfo.UserId = Convert.ToInt32(userDetailDataSet.Tables[0].Rows[0]["UserId"]);
                    userInfo.EmailType = Convert.ToInt32(userDetailDataSet.Tables[0].Rows[0]["EmailType"]);
                }
            }
            return userInfo;
        }

        /// <summary>
        /// Update user log on login
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool UpdateUserLogin(User userInfo)
        {
            _cmd = _db.GetStoredProcCommand("UpdateUserLogin");
            _db.AddInParameter(_cmd, "@UserId", DbType.Int32, userInfo.UserId);
            _db.AddInParameter(_cmd, "@ValidLogin", DbType.Int32, userInfo.ValidLogin);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, userInfo.UserName);
            return _db.ExecuteNonQuery(_cmd) == 0;
        }

        /// <summary>
        /// Save AuditLog for LogOut
        /// </summary>
        /// <param name="auditLog"></param>
        /// <returns></returns>
        public int SaveAuditLog(AuditLogReport auditLog)
        {
            _cmd = _db.GetStoredProcCommand("SaveAuditLog");
            _db.AddInParameter(_cmd, "@UserName", DbType.String, auditLog.RequestedUserName);
            _db.AddInParameter(_cmd, "@Description", DbType.String, auditLog.Description);
            return _db.ExecuteNonQuery(_cmd);
        }

        /// <summary>
        /// Get User Setting
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public byte GetUserLandingPage(int userId)
        {
            _cmd = _db.GetStoredProcCommand("GetUserLandingPage");
            _db.AddInParameter(_cmd, "@UserID", DbType.Int32, userId);
            return GetValue<byte>(_db.ExecuteScalar(_cmd), typeof(byte));
        }

        /// <summary>
        /// Save User Setting
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool SaveUserLandingPage(User userInfo)
        {
            _cmd = _db.GetStoredProcCommand("SaveUserLandingPage");
            _db.AddInParameter(_cmd, "@UserID", DbType.Int32, userInfo.UserId);
            _db.AddInParameter(_cmd, "@LandingPageID", DbType.Byte, userInfo.LandingPageId);
            return _db.ExecuteNonQuery(_cmd) > 0;
        }

        /// <summary>
        /// Disposes the objects
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _cmd.Dispose();
            _db = null;
        }
    }
}
