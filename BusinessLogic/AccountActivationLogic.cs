using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class AccountActivationLogic
    {
        /// <summary>
        /// The _accountActivationRepository
        /// </summary>
         private readonly IAccountActivationRepository _accountActivationRepository;
     
         /// <summary>
         /// Initializes a new instance of the <see cref="AccountActivationLogic"/> class.
         /// </summary>
        public AccountActivationLogic()
        {
            _accountActivationRepository = Factory.CreateInstance<IAccountActivationRepository>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountActivationLogic"/> class.
        /// </summary>
        public AccountActivationLogic(IAccountActivationRepository accountActivation)
        {
            if (accountActivation != null)
                _accountActivationRepository = accountActivation;
        }

        /// <summary>
        /// Get list of question.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<SecurityQuestion> GetSecurityQuestion(User user)
        {
            return _accountActivationRepository.GetSecurityQuestion(user);
        }

        /// <summary>
        /// Validate email exist or not.
        /// </summary>
        public int IsUserEmailExist(User user)
        {
            return _accountActivationRepository.IsUserEmailExist(user);
        }

        /// <summary>
        /// Add question answer and password.
        /// </summary>
        /// <returns></returns>
        public int AddQuestionAnswerAndPassword(User user)
        {
            return _accountActivationRepository.AddQuestionAnswerAndPassword(user);
        }

        /// <summary>
        /// Validate email exist or not and locked or not.
        /// </summary>
        public int IsUserEmailLockedOrNot(User user)
        {
            return _accountActivationRepository.IsUserEmailLockedOrNot(user);
        }

    }
}
