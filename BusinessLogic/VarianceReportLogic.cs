using System;
using System.Globalization;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class VarianceReportLogic
    {
        /// <summary>
        /// The _variance report repository
        /// </summary>
        private readonly IVarianceReportRepository _varianceReportRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VarianceReportLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public VarianceReportLogic(string connectionString)
        {
            _varianceReportRepository = Factory.CreateInstance<IVarianceReportRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VarianceReportLogic"/> class.
        /// </summary>
        /// <param name="varianceReportRepository">The variance report repository.</param>
        public VarianceReportLogic(IVarianceReportRepository varianceReportRepository)
        {
            if (varianceReportRepository != null)
            {
                _varianceReportRepository = varianceReportRepository;
            }
        }

        /// <summary>
        /// Gets all variance report.
        /// </summary>
        /// <param name="varianceReport">The variance report.</param>
        /// <returns>reportInfo</returns>
        public VarianceReport GetVarianceReport(VarianceReport varianceReport)
        {
            var reportInfo = new VarianceReport();
            if (varianceReport != null)
            {
                if (varianceReport.StartDate == DateTime.MinValue &&
                     varianceReport.EndDate == DateTime.MinValue)
                {
                    if (!string.IsNullOrEmpty(varianceReport.ClaimSearchCriteria) && varianceReport.ClaimSearchCriteria.Contains(Constants.AdjudicationRequestCriteria))
                        varianceReport.DateType = -1;

                    varianceReport.EndDate = DateTime.Now;
                    varianceReport.StartDate =
                        DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
                }

                //when file type is excel and report level is claim level, call GetExcelVarianceReport. In any other case use GetAllVarianceReport
                reportInfo = varianceReport.FileType == Constants.DownloadFileTypeExcel && varianceReport.ReportLevel == Constants.ReportLevelClaim
                    ? _varianceReportRepository.GetExcelVarianceReport(varianceReport, GlobalConfigVariable.MaxRecordLimitForExcelReport)
                    : _varianceReportRepository.GetAllVarianceReport(varianceReport);

                reportInfo.ReportDate = DateTime.UtcNow.ToString(Constants.DateTime24HourFormat, CultureInfo.InvariantCulture);
                reportInfo.DateType = varianceReport.DateType;
            }
            return reportInfo;
        }
    }
}
