using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using SSI.ContractManagement.Web.Report;
using System;
using System.IO;

namespace SSI.ContractManagement.Web.WebUtil
{
    public class ClaimAdjudicationReportUtil
    {

        private readonly ReportUtility _reportUtility;
        public ClaimAdjudicationReportUtil()
        {
            _reportUtility = new ReportUtility();
        }

        public string GetExportedFileName(ClaimAdjudicationReportViewModel adjudicationList, Enums.DownloadFileType reportFormat, string varianceReportVirtualPath, string currentDateTime)
        {
            string fileName;
            if (adjudicationList.ReportThreshold == Constants.ReportThreshold)
            {
                fileName = Convert.ToString(Constants.ReportThreshold);
            }
            else if (adjudicationList.ClaimAdjudicationReports.Count == 0)
            {
                fileName = Constants.EmptyReportResult;
            }
            else
            {
                fileName = ReportUtility.CreateFileUsingTelerik(new ClaimAdjudicationDetails(adjudicationList), reportFormat, varianceReportVirtualPath, Constants.ClaimVarianceReportFileBaseName, currentDateTime);
            }
            return fileName;
        }

        /// <summary>
        /// takes different parameters and returns either file name or error result
        /// </summary>
        /// <param name="reportFormat">either pdf or excel format</param>
        /// <param name="varianceReportList">items to be written to the file</param>
        /// <param name="configuredPath">path to store the file in server</param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public string GetExportedFileName(Enums.DownloadFileType reportFormat, ClaimAdjudicationReportViewModel varianceReportList, string configuredPath, string currentDateTime)
        {
            string fileName = string.Empty;
            if (!Directory.Exists(configuredPath))
                Directory.CreateDirectory(configuredPath);

            //if threshold is Constants.ReportThreshold, report can't be displayed as number of claims exceeds the requirement. Proper message would be displayed to user 
            //in this case
            if (varianceReportList.ReportThreshold == Constants.ReportThreshold)
                fileName = Convert.ToString(Constants.ReportThreshold);
            else if (varianceReportList.ClaimData.Count == 0 && varianceReportList.ClaimCount == 0)
            {
                fileName = Constants.EmptyReportResult;
            }
            else
            {
                if (reportFormat == Enums.DownloadFileType.Xls)
                {
                    fileName = GetReportData(varianceReportList, currentDateTime);
                }
            }
            return fileName;
        }

        /// <summary>
        /// Prepares complete variance report string
        /// </summary>
        /// <param name="claimAdjudicationReportReportList">list of claim data in variance report</param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        private string GetReportData(ClaimAdjudicationReportViewModel claimAdjudicationReportReportList, string currentDateTime)
        {
            string fileName = (claimAdjudicationReportReportList.ColumnNames != null && claimAdjudicationReportReportList.ColumnNames.Count > 0)
                ? ReportUtility.GenerateCsvFileForOpenClaims(claimAdjudicationReportReportList.ClaimData, claimAdjudicationReportReportList.ColumnNames, currentDateTime) :
                _reportUtility.GenerateCsvFile(claimAdjudicationReportReportList.ClaimData, currentDateTime);
            return fileName;
        }
    }
}