using SSI.ContractManagement.Shared.Models;
using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IAccountActivationRepository : IDisposable
    {
        /// <summary>
        /// Get list of question.
        /// </summary>
        List<SecurityQuestion> GetSecurityQuestion(User user);

        /// <summary>
        /// Validate email exist or not.
        /// </summary>
        int IsUserEmailLockedOrNot(User user);


        /// <summary>
        /// Validate email exist or not.
        /// </summary>
        int IsUserEmailExist(User user);

        /// <summary>
        /// Add question answer and password.
        /// </summary>
        /// <returns></returns>
        int AddQuestionAnswerAndPassword(User user);
    }
}
