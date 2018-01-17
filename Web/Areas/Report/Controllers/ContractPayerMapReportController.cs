using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using SSI.ContractManagement.Web.Report;
using SSI.ContractManagement.Web.WebUtil;
using System;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{
    public class ContractPayerMapReportController : CommonController
    {
        public ActionResult PayerMapping(long? nodeId, int reportType, string currentDateTime)
        {
            string fileName = string.Empty;
            ContractPayerMapReport contractPayerMapReport = new ContractPayerMapReport
            {
                NodeId = nodeId,
                ReportType = Constants.PayerMapping[reportType],
                CommandTimeoutForModelingReport = Convert.ToInt32(GlobalConfigVariable.CommandTimeout),
                LoggedInUser = GetLoggedInUserName()
            };

            var mappedReport = PostApiResponse<ContractPayerMapReport>(Constants.ContractPayerMapReport,
                Convert.ToString(Enums.Action.Get),
                contractPayerMapReport);

            var payerMapReportViewModel =
                AutoMapper.Mapper.Map<ContractPayerMapReport, ContractPayerMapReportViewModel>(mappedReport);
            payerMapReportViewModel.NodeId = nodeId;
            payerMapReportViewModel.LoggedInUser = GetCurrentUserName();
            // Gets local timeZone string.
            payerMapReportViewModel.CurrentDateTime = Utilities.GetLocalTimeString(currentDateTime);
            if (payerMapReportViewModel.ContractPayerMapReportViewModels != null && payerMapReportViewModel.ContractPayerMapReportViewModels.Count > 0)
            {
                fileName = ReportUtility.CreateFileUsingTelerik(new PayerMapping(payerMapReportViewModel), Enums.DownloadFileType.Pdf,
                    GlobalConfigVariable.ReportsFilePath, Constants.PayerMappingReportFileBaseName, currentDateTime);
            }
            return Json(fileName);
        }
    }
}
