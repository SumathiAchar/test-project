using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using SSI.ContractManagement.Web.Report;
using System;
using System.IO;
using System.Web.Mvc;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{
    public class ModelingReportController : CommonController
    {
        public ActionResult ModelingReport(long? nodeId, bool isActive, int? fileType, string currentDateTime)
        {
            string fileName = string.Empty;
            string reportFormat = fileType == 1
                ? Enums.DownloadFileType.Pdf.ToString()
                : Enums.DownloadFileType.Xls.ToString();

            ModelingReport modelingReport = new ModelingReport
            {
                NodeId = nodeId,
                IsActive = isActive,
                UserName = GetCurrentUserName(),
                CommandTimeoutForModelingReport = Convert.ToInt32(GlobalConfigVariable.CommandTimeout),
                LoggedInUser = GetLoggedInUserName()
            };


            ModelingReport modelingReportInfo = PostApiResponse<ModelingReport>(Constants.ModelingReport,
                Constants.GetAllModelingDetails,
                modelingReport);

            ModelingReportViewModel modellingReportlist =
                AutoMapper.Mapper.Map<ModelingReport, ModelingReportViewModel>(modelingReportInfo);
            modellingReportlist.NodeId = nodeId;
            modellingReportlist.LoggedInUser = GetCurrentUserName();

            // Gets the current CST time.
            modellingReportlist.CurrentDateTime = Utilities.GetLocalTimeString(currentDateTime);
            if (modellingReportlist.ModelingReports != null && modellingReportlist.ModelingReports.Count > 0)
            {
                fileName = Export(new ContractModeling(modellingReportlist), reportFormat);
            }
            return Json(fileName);
        }

        /// <summary>
        /// Export report results in a given format
        /// </summary>
        /// <param name="reportToExport">Holds report object</param>
        /// <param name="reportFormat">Hold report format</param>
        /// <returns>Returns generated file name</returns>
        private string Export(Telerik.Reporting.Report reportToExport, string reportFormat)
        {
            var reportProcessor = new ReportProcessor();
            var instanceReportSource = new InstanceReportSource { ReportDocument = reportToExport };
            RenderingResult result = reportProcessor.RenderReport(reportFormat, instanceReportSource, null);

            string dateTimeStamp = DateTime.Now.ToString(Constants.DateTimeExtendedFormat);
            string fileName = string.Format("{0}{1}.{2}", Constants.ModelingReportFileBaseName, dateTimeStamp, result.Extension);

            string path = GlobalConfigVariable.ReportsFilePath;
            string filePath = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
            }
            return fileName;
        }

        /// <summary>
        /// Downloads the report.
        /// </summary>
        /// <param name="reportFileName">Name of the report file.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="isActive">if set to <c>true</c> [is active].</param>
        /// <param name="currentDateTime">The time zone information.</param>
        /// <returns></returns>
        public ActionResult DownloadReport(string reportFileName, int? fileType, bool isActive, string currentDateTime)
        {
            string contentType;
            string fileName;

            string path = GlobalConfigVariable.ReportsFilePath;
            string filePath = Path.Combine(path, reportFileName);
            string fileNameWithoutExtension = string.Format("{0}-{1}{2}.{3}", Constants.ModelingReportFileBaseName,
                isActive ? Constants.ActiveReport : Constants.InActiveReport, currentDateTime, "{0}");

            if (fileType == 1)
            {
                contentType = MimeAssistant.GetMimeTypeByExtension(Enums.DownloadFileType.Pdf.ToString());
                fileName = string.Format(fileNameWithoutExtension, Enums.DownloadFileType.Pdf);
            }
            else
            {
                contentType = MimeAssistant.GetMimeTypeByExtension(Enums.DownloadFileType.Xls.ToString());
                fileName = string.Format(fileNameWithoutExtension, Enums.DownloadFileType.Xls);
            }
            return File(filePath, contentType, fileName);
        }
    }
}
