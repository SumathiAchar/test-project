using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.WebUtil
{
    public class AuditLogReportUtil
    {
        /// <summary>
        /// Gets the name of the exported file.
        /// </summary>
        /// <param name="auditLogReportViewModel">The audit log report view model.</param>
        /// <param name="configuredPath">The configured path.</param>
        /// <returns></returns>
        public static string GetExportedFileName(AuditLogReportViewModel auditLogReportViewModel, string configuredPath)
        {
            if (!Directory.Exists(configuredPath))
                Directory.CreateDirectory(configuredPath);
             string fileName;
             if (auditLogReportViewModel.MaxLinesForCsvReport < 0)
            {
                return Convert.ToString(auditLogReportViewModel.MaxLinesForCsvReport);
            }
            switch (auditLogReportViewModel.AuditLogReportList.Count)
            {
                case 0:
                    fileName = Constants.EmptyReportResult;
                    break;
                default:
                    fileName = GenerateCsvFile(auditLogReportViewModel.AuditLogReportList);
                    break;
            }
            return fileName;
        }

        /// <summary>
        /// Generates the CSV file.
        /// </summary>
        /// <param name="auditLogReportViewModelList">The audit log report view model list.</param>
        /// <returns></returns>
        private static string GenerateCsvFile(IEnumerable<AuditLogReportViewModel> auditLogReportViewModelList)
        {
            string stamp = string.Format("{0}{1}.{2}", Constants.AuditLogReportFileBaseName,
                DateTime.Now.ToString(Constants.DateTimeExtendedFormat),
                Enums.DownloadFileType.Csv);
            string fileName = GlobalConfigVariable.ReportsFilePath + Constants.FileNameSeperator + stamp;

            if (!File.Exists(fileName))
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    writer.WriteLine(Constants.AuditLogReportHeader);
                    foreach (AuditLogReportViewModel auditLogReport in auditLogReportViewModelList)
                    {
                        IEnumerable<string> stringRowValues = GetStringRowValues(auditLogReport);
                        List<string> rowValues = stringRowValues.ToList();
                        int count = rowValues.Count;
                        for (int index = 0; index < rowValues.Count; index++)
                        {
                            if (index == count - 1)
                            {
                                stringBuilder.Append(!string.IsNullOrEmpty(rowValues[index])
                                    ? rowValues[index].Replace(Constants.CarriageReturn, string.Empty)
                                        .Replace(Constants.NewLine, string.Empty)
                                        .Replace(Constants.Comma, Constants.SemiColon)
                                    : string.Empty);
                                stringBuilder.Append(Constants.NewLine);
                            }
                            else
                            {
                                string amountRowValue = rowValues[index].Replace(Constants.Comma, Constants.SemiColon);
                                AddComma(string.IsNullOrEmpty(amountRowValue)
                                    ? string.Empty
                                    : amountRowValue, stringBuilder);
                            }
                        }
                    }
                    writer.WriteLine(stringBuilder.ToString());
                }
            }
            return fileName;
        }


        /// <summary>
        /// Adds the comma.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="stringBuilder">The string builder.</param>
        private static void AddComma(string value, StringBuilder stringBuilder)
        {
            if (value != null)
                stringBuilder.Append(value.Contains(Constants.Currency)
                    ? value.Replace(Constants.Comma, string.Empty)
                    : value.Replace(Constants.Comma, Constants.Space));
            stringBuilder.Append(Constants.Comma + Constants.Space);
        }

        /// <summary>
        /// Gets the string row values.
        /// </summary>
        /// <param name="auditLogReport">The audit log report.</param>
        /// <returns></returns>
        private static IEnumerable<string> GetStringRowValues(AuditLogReportViewModel auditLogReport)
        {
            return new[]
            {
                auditLogReport.AuditLogId.ToString(CultureInfo.InvariantCulture),
                auditLogReport.LoggedDate.ToString(CultureInfo.InvariantCulture),
                auditLogReport.UserName,
                auditLogReport.Action,
                auditLogReport.ObjectType, auditLogReport.FacilityName, auditLogReport.ModelName,
                auditLogReport.ContractName, auditLogReport.ServiceTypeName, auditLogReport.Description
            };
        }
    }
}
