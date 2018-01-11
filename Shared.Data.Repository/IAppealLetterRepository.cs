using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IAppealLetterRepository : IDisposable
    {

        /// <summary>
        /// Gets the appeal letter.
        /// </summary>
        /// <param name="appealLetter">The appeal letter container.</param>
        /// <returns></returns>
        AppealLetter GetAppealLetter(AppealLetter appealLetter);


        /// <summary>
        /// Gets the appeal templates.
        /// </summary>
        /// <returns></returns>
        List<LetterTemplate> GetAppealTemplates(LetterTemplate appealLetterInfo);
    }
}
