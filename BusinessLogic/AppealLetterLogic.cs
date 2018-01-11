using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class AppealLetterLogic
    {
        /// <summary>
        /// The _appeal letter repository
        /// </summary>
        private readonly IAppealLetterRepository _appealLetterRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AppealLetterLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public AppealLetterLogic(string connectionString)
        {
            _appealLetterRepository = Factory.CreateInstance<IAppealLetterRepository>(connectionString, true);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AppealLetterLogic"/> class.
        /// </summary>
        /// <param name="appealLetterRepository">The appeal letter repository.</param>
        public AppealLetterLogic(IAppealLetterRepository appealLetterRepository)
        {
            if (appealLetterRepository != null)
            {
                _appealLetterRepository = appealLetterRepository;
            }
        }

        /// <summary>
        /// Gets the appeal letter.
        /// </summary>
        /// <param name="appealLetter">The appeal letter container.</param>
        /// <returns></returns>
        public AppealLetter GetAppealLetter(AppealLetter appealLetter)
        {
            //if Start date and end date are mindate than update date criteria as by default 3 years(based on configuration)
            if (appealLetter!= null && appealLetter.StartDate == DateTime.MinValue &&
                      appealLetter.EndDate == DateTime.MinValue)
            {
                if (!string.IsNullOrEmpty(appealLetter.ClaimSearchCriteria) && appealLetter.ClaimSearchCriteria.Contains(Constants.AdjudicationRequestCriteria))
                    appealLetter.DateType = Constants.DefaultDateType;

                appealLetter.EndDate = DateTime.Now;
                appealLetter.StartDate =
                    DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
            }
                return _appealLetterRepository.GetAppealLetter(appealLetter);
        }

        /// <summary>
        /// Gets the appeal templates.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<LetterTemplate> GetAppealTemplates(LetterTemplate appealLetterInfo)
        {
            return _appealLetterRepository.GetAppealTemplates(appealLetterInfo);
        }

    }
}
