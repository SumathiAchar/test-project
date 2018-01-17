using AutoMapper;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using SSI.ContractManagement.Web.WebUtil;
using System;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{
    public class ClaimAdjudicationReportController : CommonController
    {
        /// <summary>
        /// Claims the adjudication report.
        /// </summary>
        /// <param name="modelId">The model unique identifier.</param>
        /// <param name="dateType">The dateType.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="currentDateTime"></param>
        /// <returns>ActionResult.</returns>
        public ActionResult ClaimAdjudicationReport(long modelId, int? dateType, DateTime? startDate, DateTime? endDate, string criteria, string currentDateTime)
        {
            string currentUserName = GetLoggedInUserName();
            ClaimAdjudicationReport claimAdjudicationReport = new ClaimAdjudicationReport
                 {
                     ModelId = modelId,
                     NodeId = modelId,
                     DateType = Convert.ToInt32(dateType),
                     StartDate = Convert.ToDateTime(startDate),
                     EndDate = Convert.ToDateTime(endDate),
                     ClaimSearchCriteria = criteria,
                     FacilityId = GetCurrentFacilityId(),
                     //Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
                     RequestedUserId =
                         string.IsNullOrEmpty(currentUserName) ? Guid.Empty.ToString() : GetUserKey(),
                     RequestedUserName = currentUserName,
                     CommandTimeoutForClaimAdjudication =
                         Convert.ToInt32(GlobalConfigVariable.CommandTimeoutForClaimAdjudication),
                     MaxLinesForPdfReport =
                         GlobalConfigVariable.MaxRecordLimitForTelericReport,
                 };
            ClaimAdjudicationReport claimAdjudicationReportDetails =
                PostApiResponse<ClaimAdjudicationReport>(Constants.ClaimAdjudicationReport,
                    Constants.GetClaimAdjudicationReport,
                    claimAdjudicationReport);
            ClaimAdjudicationReportViewModel adjudicationList =
                                Mapper.Map<ClaimAdjudicationReport, ClaimAdjudicationReportViewModel>(
                                    claimAdjudicationReportDetails);


            // Gets the current CST time.
            adjudicationList.CurrentDateTime = Utilities.GetLocalTimeString(currentDateTime);
            adjudicationList.LoggedInUser = currentUserName;
            string fileName = new ClaimAdjudicationReportUtil().GetExportedFileName(adjudicationList, Enums.DownloadFileType.Pdf, GlobalConfigVariable.ReportsFilePath, currentDateTime);
            return Json(fileName);
        }

        /// <summary>
        /// Download the report for a requested format
        /// </summary>
        /// <param name="reportFileName">Holds report file name</param>
        /// <param name="currentDateTime"></param>
        /// <returns>files result. is used to download the file</returns>
        public ActionResult DownloadReport(string reportFileName, string currentDateTime)
        {
            string fileName;
            string contentType;
            string filePath = ReportUtility.GetReportFileDetailsToDownload(Constants.ClaimVarianceReportFileBaseName, reportFileName, out fileName, Enums.DownloadFileType.Pdf, out contentType, currentDateTime);
            return File(filePath, contentType, fileName);
        }
    }
}
