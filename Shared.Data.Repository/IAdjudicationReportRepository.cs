using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IAdjudicationReportRepository : IDisposable
    {
        /// <summary>
        /// Gets all claim adjudication.
        /// </summary>
        /// <param name="claimAdjudicationReport">The claim adjudication report.</param>
        /// <returns></returns>
        ClaimAdjudicationReport GetClaimAdjudicationReport(ClaimAdjudicationReport claimAdjudicationReport);

        /// <summary>
        /// Gets the selected claim.
        /// </summary>
        /// <param name="claimAdjudicationReport">The claim adjudication report.</param>
        /// <param name="maxRecordLimitForExcelReport">The maximum record limit for excel report.</param>
        /// <returns></returns>
        ClaimAdjudicationReport GetSelectedClaim(ClaimAdjudicationReport claimAdjudicationReport, int maxRecordLimitForExcelReport);

        /// <summary>
        /// Gets Open Claim Column Names By UserId
        /// </summary>
        /// <param name="claimAdjudicationReport">The data.</param>
        /// <returns></returns>
        ClaimAdjudicationReport GetOpenClaimColumnNamesBasedOnUserId(ClaimAdjudicationReport claimAdjudicationReport);
    }
}
