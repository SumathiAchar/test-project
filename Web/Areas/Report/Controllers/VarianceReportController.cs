using AutoMapper;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using SSI.ContractManagement.Web.WebUtil;
using System;
using System.IO;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{

    public class VarianceReportController : CommonController
    {
        /// <summary>
        /// Variances the claim report.
        /// </summary>
        /// <param name="nodeId">The node unique identifier.</param>
        /// <param name="datetype">The datetype.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="reportType">Type of the report.</param> 
        /// <param name="fileType">type of file. Either pdf or excel</param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public ActionResult VarianceReport(long? nodeId, int? datetype, DateTime? startDate, DateTime? endDate, string criteria, int reportType, int fileType, string currentDateTime)
        {
            string fileName;
            Enums.DownloadFileType reportFormat = fileType == 1 ? Enums.DownloadFileType.Pdf : Enums.DownloadFileType.Xls;
            //If the Report Type is Claim Level and report format is PDF
            if (reportType == Constants.One && fileType == Constants.One)
            {
                ClaimAdjudicationReport claimAdjudicationReport = new ClaimAdjudicationReport
                {
                    ModelId = nodeId,
                    NodeId = nodeId,
                    DateType = Convert.ToInt32(datetype),
                    StartDate = Convert.ToDateTime(startDate),
                    EndDate = Convert.ToDateTime(endDate),
                    ClaimSearchCriteria = criteria,
                    FacilityId = GetCurrentFacilityId(),
                    //Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
                    RequestedUserId =
                        string.IsNullOrEmpty(GetCurrentUserName()) ? Guid.Empty.ToString() : GetUserKey(),
                    RequestedUserName = GetCurrentUserName(),
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
                adjudicationList.LoggedInUser = GetCurrentUserName();
                fileName = new ClaimAdjudicationReportUtil().GetExportedFileName(adjudicationList,
                    Enums.DownloadFileType.Pdf, GlobalConfigVariable.ReportsFilePath, currentDateTime);
            }
            else
            {
                VarianceReport varianceReport = new VarianceReport
                {
                    NodeId = nodeId,
                    DateType = datetype,
                    StartDate = Convert.ToDateTime(startDate),
                    EndDate = Convert.ToDateTime(endDate),
                    ClaimSearchCriteria = criteria,
                    ReportLevel = reportType,
                    FacilityId = GetCurrentFacilityId(),
                    RequestedUserId = string.IsNullOrEmpty(GetCurrentUserName()) ? Guid.Empty.ToString() : GetUserKey(),
                    RequestedUserName = GetCurrentUserName(),
                    FileType = fileType,
                    MaxLinesForPdfReport = GlobalConfigVariable.MaxRecordLimitForTelericReport
                };

                //Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
                VarianceReport varianceReportInfo = PostApiResponse<VarianceReport>(Constants.VarianceReport,
                    Constants.GetVarianceReport, varianceReport);
                VarianceReportViewModel varianceReportlist =
                    Mapper.Map<VarianceReport, VarianceReportViewModel>(varianceReportInfo);
                varianceReportlist.LoggedInUser = GetCurrentUserName();
                // Gets the local time zone.
                varianceReportlist.CurrentDateTime = Utilities.GetLocalTimeString(currentDateTime);
                fileName = new VarianceReportUtil().GetExportedFileName(reportType, reportFormat,
                    varianceReportlist, GlobalConfigVariable.ReportsFilePath, currentDateTime);
            }
            return Json(new { fileName, reportFormat}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Downloads the report.
        /// </summary>
        /// <param name="reportFileName">Name of the report file.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="currentDateTime">The time zone information.</param>
        /// <returns></returns>
        public ActionResult DownloadReport(string reportFileName, int fileType, int reportType, string currentDateTime)
        {
            string contentType;
            string fileName;

            string path = GlobalConfigVariable.ReportsFilePath;
            string filePath = Path.Combine(path, reportFileName);

            string fileNameWithoutExtension = string.Format("{0}-{1}.{2}",
                reportType ==
                Constants.ReportLevelClaim
                    ? Constants.ClaimVarianceReportFileBaseName
                    : Constants.ContractVarianceReportFileBaseName, currentDateTime, "{0}");

            if (fileType == Constants.DownloadFileTypeExcel)
            {
                var fileExtension = Path.GetExtension(reportFileName);
                string newExtenstion = string.Empty;
                if (fileExtension != null)
                {
                    newExtenstion = fileExtension.Replace(".", "");

                }
                contentType = MimeAssistant.GetMimeTypeByExtension(newExtenstion);
                fileName = string.Format(fileNameWithoutExtension, newExtenstion);
            }
            else
            {
                contentType = MimeAssistant.GetMimeTypeByExtension(Enums.DownloadFileType.Pdf.ToString());
                fileName = string.Format(fileNameWithoutExtension, Enums.DownloadFileType.Pdf);
            }
            return File(filePath, contentType, fileName);
        }
    }
}


