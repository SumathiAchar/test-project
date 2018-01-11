using System.Collections.Generic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ReportSelectionLogic
    {
        private readonly IReportSelectionRepository _reportSelectionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportSelectionLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ReportSelectionLogic(string connectionString)
        {
            _reportSelectionRepository = Factory.CreateInstance<IReportSelectionRepository>(connectionString, true);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLineClaimFieldLogic"/> class.
        /// </summary>
        /// <param name="reportSelectionRepository">The service line claim field repository.</param>
        public ReportSelectionLogic(IReportSelectionRepository reportSelectionRepository)
        {
            if (reportSelectionRepository != null)
            {
                _reportSelectionRepository = reportSelectionRepository;
            }
        }


        /// <summary>
        /// Gets all claim fields.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ClaimField> GetAllClaimFields(ClaimSelector reportClaimSelectorInfo)
        {
            return _reportSelectionRepository.GetAllClaimFields(reportClaimSelectorInfo);
        }


        /// <summary>
        /// Gets all ClaimField Operators.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public List<ClaimFieldOperator> GetAllClaimFieldsOperators()
        {
            return _reportSelectionRepository.GetAllClaimFieldsOperators();
        }

        /// <summary>
        /// Gets the claim reviewed option.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public List<ReviewedOptionType> GetClaimReviewedOption()
        {
            return _reportSelectionRepository.GetClaimReviewedOption();
        }

        /// <summary>
        /// Gets ajudication request names based on the model id and use rname.
        /// </summary>
        /// <returns> List of ClaimSelector</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ClaimSelector> GetAdjudicationRequestNames(ClaimSelector claimSelection)
        {
            return _reportSelectionRepository.GetAdjudicationRequestNames(claimSelection);
        }

        /// <summary>
        /// Add or edit query name.
        /// </summary>
        /// <param name="claimSelection"></param>
        /// <returns></returns>
        public int AddEditQueryName(ClaimSelector claimSelection)
        {
            return _reportSelectionRepository.AddEditQueryName(claimSelection);
        }

        /// <summary>
        /// Delete query name with criteria.
        /// </summary>
        /// <param name="claimSelector"></param>
        /// <returns></returns>
        public bool DeleteQueryName(ClaimSelector claimSelector)
        {
            return _reportSelectionRepository.DeleteQueryName(claimSelector);
        }

        /// <summary>
        /// Get Quries By Id
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public List<ClaimSelector> GetQueriesById(User userInfo)
        {
            return _reportSelectionRepository.GetQueriesById(userInfo);
        }
    }
}
