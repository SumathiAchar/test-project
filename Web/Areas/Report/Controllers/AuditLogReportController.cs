using System;
using System.Web.Mvc;
using AutoMapper;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using SSI.ContractManagement.Web.WebUtil;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{
    public class AuditLogReportController : CommonController
    {
        //FIXED-NOV15 - In place of 3 argument use AuditLogReport model which has all properties
        /// <summary>
        /// Audits the log report.
        /// </summary>
        /// <param name="auditLogReport">The audit log report.</param>
        /// <param name="currentDateTime">The current date time.</param>
        /// <returns></returns>
        public ActionResult AuditLogReport(AuditLogReport auditLogReport, string currentDateTime)
        {
            string currentUserName = GetCurrentUserName();
            auditLogReport.UserName = currentUserName;
            auditLogReport.CommandTimeoutForAuditLog =
                Convert.ToInt32(GlobalConfigVariable.CommandTimeoutForClaimAdjudication);
            auditLogReport.MaxLinesForCsvReport = Convert.ToInt32(GlobalConfigVariable.MaxRecordLimitForExcelReport);
            auditLogReport.CurrentDateTime = currentDateTime;
            auditLogReport.FacilityName = GetCurrentFacilityName();
            AuditLogReportViewModel auditLogReportViewModelList =
                Mapper.Map<AuditLogReport, AuditLogReportViewModel>(
                    PostApiResponse<AuditLogReport>(Constants.AuditLogReport,
                        Constants.GetAuditLogReport,
                        auditLogReport));

            // Gets the current CST time.
            auditLogReportViewModelList.CurrentDateTime = Utilities.GetLocalTimeString(currentDateTime);
            auditLogReportViewModelList.UserName = currentUserName;
            return Json(AuditLogReportUtil.GetExportedFileName(auditLogReportViewModelList, GlobalConfigVariable.ReportsFilePath));
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
            string filePath = ReportUtility.GetReportFileDetailsToDownload(Constants.AuditLogReportFileBaseName,
                reportFileName, out fileName, Enums.DownloadFileType.Csv, out contentType, currentDateTime);
            return File(filePath, contentType, fileName);
        }
    }
}
