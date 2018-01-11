using System;
using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IReportSelectionRepository : IDisposable
    {
        /// <summary>
        /// Gets all ClaimField Operators.
        /// </summary>
        /// <returns></returns>
        List<ClaimFieldOperator> GetAllClaimFieldsOperators();

        /// <summary>
        /// Gets all claim fields.
        /// </summary>
        /// <returns></returns>
        List<ClaimField> GetAllClaimFields(ClaimSelector reportClaimSelectorInfo);

        /// <summary>
        /// Gets the claim reviewed option.
        /// </summary>
        /// <returns></returns>
        List<ReviewedOptionType> GetClaimReviewedOption();

        /// <summary>
        /// Get adjudication request names based on the model id and user name
        /// </summary>
        /// <returns></returns>
        List<ClaimSelector> GetAdjudicationRequestNames(ClaimSelector claimSelector);

        /// <summary>
        /// Add or edit query name.
        /// </summary>
        /// <param name="claimSelection"></param>
        /// <returns></returns>
        int AddEditQueryName(ClaimSelector claimSelection);

        /// <summary>
        /// Delete query name with criteria.
        /// </summary>
        /// <param name="claimSelector"></param>
        /// <returns></returns>
        bool DeleteQueryName(ClaimSelector claimSelector);

        /// <summary>
        /// Get Quries By Id
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        List<ClaimSelector> GetQueriesById(User userInfo);
    }
}
