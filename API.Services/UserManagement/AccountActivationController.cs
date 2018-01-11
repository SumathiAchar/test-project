using System.Collections.Generic;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.UserManagement
{
    public class AccountActivationController : ApiController
    {
        readonly AccountActivationLogic _accountActivationLogic;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AccountActivationController()
        {
            _accountActivationLogic = new AccountActivationLogic();
        }

        /// <summary>
        /// Get list of question.
        /// </summary>
        [HttpPost]
        public List<SecurityQuestion> GetSecurityQuestion(User user)
        {
            return _accountActivationLogic.GetSecurityQuestion(user);
        }

        /// <summary>
        /// Validate email exist or not.
        /// </summary>
        [HttpPost]
        public int IsUserEmailExist(User user)
        {
            return _accountActivationLogic.IsUserEmailExist(user);
        }

        /// <summary>
        /// Add question answer and password.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public int AddQuestionAnswerAndPassword(User user)
        {
            return _accountActivationLogic.AddQuestionAnswerAndPassword(user);
        }

        /// <summary>
        /// Validate email exist or not and locked or not.
        /// </summary>
        [HttpPost]
        public int IsUserEmailLockedOrNot(User user)
        {
            return _accountActivationLogic.IsUserEmailLockedOrNot(user);
        }

    }
}
