using System;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ClaimAdjudicationReportLogic
    {

        /// <summary>
        /// The _claim adjudication report repository
        /// </summary>
        private readonly IAdjudicationReportRepository _adjudicationReportRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimAdjudicationReportLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ClaimAdjudicationReportLogic(string connectionString)
        {
            _adjudicationReportRepository = Factory.CreateInstance<IAdjudicationReportRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimAdjudicationReportLogic"/> class.
        /// </summary>
        /// <param name="adjudicationReportRepository">The adjudication report repository.</param>
        public ClaimAdjudicationReportLogic(IAdjudicationReportRepository adjudicationReportRepository)
        {
            if (adjudicationReportRepository != null)
            {
                _adjudicationReportRepository = adjudicationReportRepository;
            }
        }

        /// <summary>
        /// Gets all claim adjudication.
        /// </summary>
        /// <param name="claimAdjudicationReport">The claim adjudication report.</param>
        /// <returns>reportInfo</returns>
        public ClaimAdjudicationReport GetClaimAdjudicationReport(ClaimAdjudicationReport claimAdjudicationReport)
        {
            ClaimAdjudicationReport reportInfo = new ClaimAdjudicationReport();
            if (claimAdjudicationReport != null)
            {
                if (claimAdjudicationReport.StartDate == DateTime.MinValue &&
                       claimAdjudicationReport.EndDate == DateTime.MinValue)
                {
                    if (!string.IsNullOrEmpty(claimAdjudicationReport.ClaimSearchCriteria) && claimAdjudicationReport.ClaimSearchCriteria.Contains(Constants.AdjudicationRequestCriteria))
                        claimAdjudicationReport.DateType = Constants.DefaultDateType;

                    claimAdjudicationReport.EndDate = DateTime.Now;
                    claimAdjudicationReport.StartDate =
                        DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
                }

                reportInfo = _adjudicationReportRepository.GetClaimAdjudicationReport(claimAdjudicationReport);
            }
            return reportInfo;
        }

        /// <summary>
        /// Gets the selected claim.
        /// </summary>
        /// <param name="claimAdjudicationReport">The claim adjudication report.</param>
        /// <returns></returns>
        public ClaimAdjudicationReport GetSelectedClaim(ClaimAdjudicationReport claimAdjudicationReport)
        {
            return _adjudicationReportRepository.GetSelectedClaim(claimAdjudicationReport, GlobalConfigVariable.MaxRecordLimitForExcelReport);
        }
        /// <summary>
        /// Gets OpenClaim Columns By UserId
        /// </summary>
        /// <param name="claimAdjudicationReport">The data.</param>
        /// <returns></returns>
        public ClaimAdjudicationReport GetOpenClaimColumnNamesBasedOnUserId(ClaimAdjudicationReport claimAdjudicationReport)
        {
            return _adjudicationReportRepository.GetOpenClaimColumnNamesBasedOnUserId(claimAdjudicationReport);
        }
    }
}

