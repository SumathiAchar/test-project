using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.WebUtil;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Common.Controllers
{
    public class FileDownloadController : BaseController
    {
        /// <summary>
        /// Downloads the report.
        /// </summary>
        /// <param name="reportFileName">Name of the report file.</param>
        /// <param name="fileBaseName">Name of the file base.</param>
        /// <param name="downloadFileType">Type of the download file.</param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public ActionResult DownloadReport(string reportFileName, string fileBaseName, Enums.DownloadFileType downloadFileType, string currentDateTime)
        {
            string contentType;
            string fileName;
            string filePath = ReportUtility.GetReportFileDetailsToDownload(fileBaseName,
                reportFileName, out fileName, downloadFileType, out contentType, currentDateTime);
            return File(filePath, contentType, fileName);
        }
    }
}