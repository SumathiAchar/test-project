using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;


namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class AccountActivationRepository : IAccountActivationRepository
    {
        /// <summary>
        /// The _database obj variable
        /// </summary>
        private Database _db;

        /// <summary>
        /// The _database command variable
        /// </summary>
        DbCommand _cmd;

        /// <summary>
        /// Default constructor to initialize database instance
        /// </summary>
        public AccountActivationRepository()
        {
            _db = DatabaseFactory.CreateDatabase("CMMembershipConnectionString");
        }

        /// <summary>
        /// Get list of question.
        /// </summary>
        public List<SecurityQuestion> GetSecurityQuestion(User user)
        {
            List<SecurityQuestion> securityQuestions = new List<SecurityQuestion>();
            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetSecurityQuestion");
            _db.AddInParameter(_cmd, "@UserId", DbType.Int32, user.UserId);

            DataSet securityDataSet = _db.ExecuteDataSet(_cmd);
            if (securityDataSet.IsTableDataPopulated(0))
            {
                securityQuestions = (from DataRow row in securityDataSet.Tables[0].Rows
                                     select new SecurityQuestion
                                     {
                                         QuestionId = Convert.ToInt32(row["QuestionId"]),
                                         Question = Convert.ToString(row["Question"])
                                     }).ToList();
                if (securityDataSet.Tables[1].Rows.Count > 0)
                {
                    securityQuestions[0].QuestionIds = (from DataRow securityDataRows in securityDataSet.Tables[1].Rows
                                                        select Convert.ToInt32(Convert.ToString(securityDataRows["QuestionId"]))).ToList();
                }
            }
            return securityQuestions;
        }

        /// <summary>
        /// Validate email exist or not.
        /// </summary>
        // FIXED-FEB16 - Stored Procedure - Change @UserName VARCHAR(500) to VARCHAR(200). Int16 table column size is 200 only.
        //                  Remove Top 1 from select query. Not required. Query will always return one record only.
        public int IsUserEmailExist(User user)
        {
            if (user != null)
            {
                _cmd = _db.GetStoredProcCommand("IsUserEmailExist");
                _db.AddInParameter(_cmd, "@UserName", DbType.String, user.UserName);
                return Convert.ToInt16(_db.ExecuteScalar(_cmd));
            }
            return -1;
        }

        /// <summary>
        /// Add question answer and password.
        /// </summary>
        /// <returns></returns>
        public int AddQuestionAnswerAndPassword(User user)
        {
            if (user != null)
            {
                string passwordSalt = GetPasswordSalt(user);
                string passwordHash = EncryptDecryptPassword.EncryptText(string.Format("{0}{1}", user.PasswordHash, passwordSalt));
                _cmd = _db.GetStoredProcCommand("AddQuestionAnswerAndPassword");
                _db.AddInParameter(_cmd, "@UserID", DbType.Int32, user.UserId);
                _db.AddInParameter(_cmd, "@PasswordHash", DbType.String, passwordHash);
                _db.AddInParameter(_cmd, "@PasswordSalt", DbType.String, passwordSalt);
                _db.AddInParameter(_cmd, "@SecuirtyQuestionAnswer", DbType.Xml, Serializer.ToXml(user.UserSecurityQuestion));
                _db.AddInParameter(_cmd, "@EmailType", DbType.Int32, user.EmailType);
                _db.AddInParameter(_cmd, "@UserName", DbType.String, user.RequestedUserName);
                return Convert.ToInt32(_db.ExecuteScalar(_cmd));
            }
            return 0;
        }

        /// <summary>
        /// Add question answer and password.
        /// </summary>
        /// <returns></returns>
        private string GetPasswordSalt(User user)
        {
            string passwordSalt;
            if (user.EmailType == Convert.ToInt32(Enums.EmailType.AccountActivation) || user.EmailType == Convert.ToInt32(Enums.EmailType.AccountReset))
            {
                passwordSalt = EncryptDecryptPassword.GenerateSalt();
            }
            else
            {
                _cmd = _db.GetStoredProcCommand("GetPasswordSalt");
                _db.AddInParameter(_cmd, "@UserID", DbType.Int32, user.UserId);

                passwordSalt = Convert.ToString(_db.ExecuteScalar(_cmd));
            }
            return passwordSalt;

        }


        /// <summary>
        /// Validate email exist or not.
        /// </summary>
        // FIXED-FEB16 - Stored Procedure - Change @UserName VARCHAR(500) to VARCHAR(200). Int16 table column size is 200 only.
        //                  Remove Top 1 from select query. Not required. Query will always return one record only.
        public int IsUserEmailLockedOrNot(User user)
        {
            if (user != null)
            {
                _cmd = _db.GetStoredProcCommand("IsUserEmaillockedOrNot");
                _db.AddInParameter(_cmd, "@UserName", DbType.String, user.UserName);
                return Convert.ToInt16(_db.ExecuteScalar(_cmd));
            }
            return -1;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _cmd.Dispose();
            _db = null;
        }
    }
}
