using System;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using SSI.ContractManagement.Web.Report;
using System.IO;

namespace SSI.ContractManagement.Web.WebUtil
{
    public class VarianceReportUtil
    {
        private readonly ReportUtility _reportUtility;
        public VarianceReportUtil()
        {
            _reportUtility = new ReportUtility();
        }

        /// <summary>
        /// Prepares complete variance report string
        /// </summary>
        /// <param name="varianceReportList">list of claim data in variance report</param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        private string GetReportData(VarianceReportViewModel varianceReportList, string currentDateTime)
        {
            string fileName = _reportUtility.GenerateCsvFile(varianceReportList.ClaimData, currentDateTime);
            return fileName;
        }


        /// <summary>
        /// takes different parameters and returns either file name or error result
        /// </summary>
        /// <param name="reportType">either claim level or contract level report</param>
        /// <param name="reportFormat">either pdf or excel format</param>
        /// <param name="varianceReportList">items to be written to the file</param>
        /// <param name="configuredPath">path to store the file in server</param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public string GetExportedFileName(int reportType, Enums.DownloadFileType reportFormat, VarianceReportViewModel varianceReportList, string configuredPath, string currentDateTime)
        {
            string fileName = string.Empty;
            if (!Directory.Exists(configuredPath))
                Directory.CreateDirectory(configuredPath);

            switch (reportType)
            {
                case Constants.ReportLevelClaim:
                    //if threshold is Constants.ReportThreshold, report can't be displayed as number of claims exceeds the requirement. Proper message would be displayed to user 
                    //in this case
                    if (varianceReportList.CountThreshold == Constants.ReportThreshold)
                        fileName = Convert.ToString(Constants.ReportThreshold);
                    else if (varianceReportList.ClaimData.Count == 0 && varianceReportList.VarianceReports.Count == 0)
                    {
                        fileName = Constants.EmptyReportResult;
                    }
                    else
                    {
                        if (reportFormat == Enums.DownloadFileType.Xls)
                        {
                            fileName = GetReportData(varianceReportList, currentDateTime);
                        }
                        else
                        {
                            fileName = ReportUtility.CreateFileUsingTelerik(new ClaimVarianceDetails(varianceReportList),
                            reportFormat, configuredPath, Constants.VarianceReportFileBaseName, currentDateTime);
                        }
                    }
                    break;
                case Constants.ReportLevelContract:
                    if (varianceReportList.Contracts == null || varianceReportList.Contracts.Count == 0)
                        fileName = Constants.EmptyReportResult;
                    else
                    {
                        fileName = ReportUtility.CreateFileUsingTelerik(new ContractVarianceDetails(varianceReportList),
                            reportFormat, configuredPath, Constants.VarianceReportFileBaseName, currentDateTime);
                    }
                    break;
            }
            return fileName;
        }
    }
}