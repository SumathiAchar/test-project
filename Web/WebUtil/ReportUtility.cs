using System;
using Microsoft.Ajax.Utilities;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.Areas.Common.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace SSI.ContractManagement.Web.WebUtil
{
    public class ReportUtility
    {
        /// <summary>
        /// Gets the report file details to download.
        /// </summary>
        /// <param name="reportFileBaseName">Name of the report file base.</param>
        /// <param name="reportFileName">Name of the report file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileContentType">Type of the file content.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public static string GetReportFileDetailsToDownload(string reportFileBaseName, string reportFileName, out string fileName,
            Enums.DownloadFileType fileContentType, out string contentType, string currentDateTime)
        {


            string reportLocation = GlobalConfigVariable.ReportsFilePath;

            string filePath = Path.Combine(reportLocation, reportFileName);
            string fileNameWithoutExtension = string.Format("{0}-{1}.{2}", reportFileBaseName, currentDateTime, "{0}");

            switch (fileContentType)
            {
                case Enums.DownloadFileType.Xls:
                    contentType = MimeAssistant.GetMimeTypeByExtension(Enums.DownloadFileType.Xls.ToString());
                    fileName = string.Format(fileNameWithoutExtension, Enums.DownloadFileType.Xls);
                    break;
                case Enums.DownloadFileType.Csv:
                    contentType = MimeAssistant.GetMimeTypeByExtension(Enums.DownloadFileType.Csv.ToString());
                    fileName = string.Format(fileNameWithoutExtension, Enums.DownloadFileType.Csv);
                    break;
                case Enums.DownloadFileType.Rtf:
                    contentType = MimeAssistant.GetMimeTypeByExtension(Enums.DownloadFileType.Rtf.ToString());
                    fileName = string.Format(fileNameWithoutExtension, Enums.DownloadFileType.Rtf);
                    break;
                default:
                    contentType = MimeAssistant.GetMimeTypeByExtension(Enums.DownloadFileType.Pdf.ToString());
                    fileName = string.Format(fileNameWithoutExtension, Enums.DownloadFileType.Pdf);
                    break;
            }
            return filePath;
        }

        /// <summary>
        /// preparing rows for variance claim report
        /// </summary>
        /// <param name="reportList">The report list.</param>
        /// <param name="currentDateTime"> </param>
        /// <returns>
        /// a string of all rows for variance report
        /// </returns>
        public string GenerateCsvFile(IEnumerable<ClaimDataViewModel> reportList, string currentDateTime)
        {
            string stamp = string.Format("{0}{1}.{2}", Constants.VarianceReportFileBaseName, currentDateTime,
                 Enums.DownloadFileType.Csv);
            string fileName = GlobalConfigVariable.ReportsFilePath + Constants.FileNameSeperator + stamp;

            if (!File.Exists(fileName))
            {
                using (var writer = new StreamWriter(fileName))
                {
                    var stringBuilder = new StringBuilder();

                    writer.WriteLine(Constants.ClientHeader);
                    foreach (var claimData in reportList)
                    {

                        IEnumerable<string> stringRowValuesFromClaimIdToPatientAccountNumber = GetStringRowValuesFromClaimIdToPatientAccountNumber(claimData);
                        IEnumerable<string> stringRowValuesFromNpiToMrn = GetStringRowValuesFromNpiToMrn(claimData);
                        string stringAgeRowValues = GetAgeRowValues(claimData);
                        IEnumerable<double?> amountRowValues = GetAmountRowValues(claimData);
                        IList<string> dateTypeRowValues = GetDateTypeRowValues(claimData).ToList();
                        string stringLosRowValues = GetLosRowValues(claimData);
                        IList<string> dateTypeRowValuesAfterLos = GetDateTypeRowValuesAfterLos(claimData).ToList();

                        string claimStateValue = claimData.ClaimState;

                        foreach (var rowValue in stringRowValuesFromClaimIdToPatientAccountNumber)
                        {
                            AddComma(rowValue, stringBuilder);
                        }

                        //Adds the Los value to the stringBuilder
                        AddComma(stringAgeRowValues, stringBuilder);

                        foreach (var rowValue in stringRowValuesFromNpiToMrn)
                        {
                            AddComma(rowValue, stringBuilder);
                        }

                        foreach (var rowValue in amountRowValues)
                        {
                            AddComma(
                                rowValue == null
                                    ? string.Empty
                                    : decimal.Parse(rowValue.ToString()).ToString("C2"), stringBuilder);
                        }

                        //Adds the Claim state value to the stringBuilder
                        AddComma(claimStateValue, stringBuilder);

                        for (int dateTypeRowIndex = 0; dateTypeRowIndex < dateTypeRowValues.Count; dateTypeRowIndex++)
                        {
                            if (dateTypeRowValues[dateTypeRowIndex] == Constants.DateTime1900)
                                dateTypeRowValues[dateTypeRowIndex] = null;
                            AddComma(dateTypeRowValues[dateTypeRowIndex], stringBuilder);
                        }

                        //Adds the Los value to the stringBuilder
                        AddComma(stringLosRowValues, stringBuilder);

                        GetRowValuesFromBillDateToLastFilledDate(dateTypeRowValuesAfterLos, stringBuilder);
                    }
                    writer.WriteLine(stringBuilder.ToString());
                }
            }

            return fileName;
        }
        /// <summary>
        /// Preparing rows for Open Claims
        /// </summary>
        /// <param name="reportList">The report list.</param>
        /// <param name="claimColumnNames"> </param>
        /// <param name="currentDateTime"> </param>
        /// <returns>
        /// a string of all rows for Adjudication report
        /// </returns>
        public static string GenerateCsvFileForOpenClaims(IEnumerable<ClaimDataViewModel> reportList, IEnumerable<string> claimColumnNames, string currentDateTime)
        {
            string stamp = string.Format("{0}{1}.{2}", Constants.VarianceReportFileBaseName, currentDateTime,
                 Enums.DownloadFileType.Csv);
            string fileName = GlobalConfigVariable.ReportsFilePath + Constants.FileNameSeperator + stamp;

            if (!File.Exists(fileName))
            {
                using (var writer = new StreamWriter(fileName))
                {
                    var stringBuilder = new StringBuilder();
                    List<string> columnNames = new List<string> { Constants.IsReviewed };
                    foreach (string columnName in claimColumnNames)
                    {
                        switch (columnName)
                        {
                            case Constants.ActualContractualAdjustment:
                                columnNames.Add(Constants.ActualAdjustment);
                                break;
                            case Constants.ExpectedContractualAdjustment:
                                columnNames.Add(Constants.CalculatedAdjustment);
                                break;
                            default:
                                columnNames.Add(columnName);
                                break;
                        }
                    }
                    //writing Column Names as Header
                    writer.WriteLine(string.Join(Constants.CommaWithSpace, columnNames));
                    foreach (var claimData in reportList)
                    {
                        foreach (string columnName in columnNames)
                        {
                            AddComma(
                                columnName != columnNames[columnNames.Count - 1]
                                    ? GetRowValuesFromClaimData(columnName, claimData)
                                    : GetRowValuesFromClaimData(columnName, claimData).TrimEnd(Constants.CommaCharacter), stringBuilder, columnName);
                        }

                        stringBuilder.Append(Constants.NewLine);
                    }
                    writer.WriteLine(stringBuilder.ToString());
                }
            }

            return fileName;
        }
        /// <summary>
        /// Check Given claim columns is amount column
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private static bool IsAmountColumn(string columnName)
        {
            bool isAmountColumn = false;
            switch (columnName)
            {
                case Constants.PropertyTotalCharges:
                case Constants.AdjudicatedValue:
                case Constants.ActualPayment:
                case Constants.ClaimPatientResponsibility:
                case Constants.RemitAllowedAmt:
                case Constants.RemitNonCovered:
                case Constants.CalculatedAdjustment:
                case Constants.ActualAdjustment:
                case Constants.ContractualVariance:
                case Constants.PaymentVariance:
                    isAmountColumn = true;
                    break;
            }

            return isAmountColumn;
        }
        /// <summary>
        /// GetRowValues From ClaimData Based on Column name
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="claimData"></param>
        /// <returns></returns>
        private static string GetRowValuesFromClaimData(string columnName, ClaimDataViewModel claimData)
        {
            string rowValue = string.Empty, fieldName = string.Empty;
            switch (columnName)
            {
                case Constants.ClaimId:
                    rowValue = claimData.ClaimId.ToString(CultureInfo.InvariantCulture);
                    break;
                case Constants.IsReviewed:
                    rowValue = claimData.IsReviewed ? Constants.ReviewedOptionYes : Constants.ReviewedOptionNo;
                    break;
                case Constants.PropertyPriPayerName:
                    rowValue = claimData.PriPayerName.Replace(Constants.Comma, Constants.SemiColon);
                    break;
                case Constants.SecPayerName:
                    rowValue = claimData.SecPayerName.Replace(Constants.Comma, Constants.SemiColon);
                    break;
                case Constants.TerPayerName:
                    rowValue = claimData.TerPayerName.Replace(Constants.Comma, Constants.SemiColon);
                    break;
            }
            //Setting Database column name to Property Name
            switch (columnName)
            {
                case Constants.ClaimId:
                    fieldName = Constants.ClaimIdValue;
                    break;
                case Constants.ClaimStatus:
                    fieldName = Constants.ClaimStat;
                    break;
                case Constants.SsiNumberWithCaps:
                    fieldName = Constants.SsiNumber;
                    break;
                case Constants.NpiCaps:
                    fieldName = Constants.PropertyNpi;
                    break;
                case Constants.DrgWithCaps:
                    fieldName = Constants.PropertyDrg;
                    break;
                case Constants.PriIcddCodeCaps:
                    fieldName = Constants.PriIcddCode;
                    break;
                case Constants.PriIcdPCodeCaps:
                    fieldName = Constants.PriIcdpCode;
                    break;
                case Constants.MemberIdCaps:
                    fieldName = Constants.MemberId;
                    break;
                case Constants.IcnWithCaps:
                    fieldName = Constants.PropertyIcn;
                    break;
                case Constants.MrnWithCaps:
                    fieldName = Constants.PropertyMrn;
                    break;
                case Constants.AdjudicatedDate:
                    fieldName = Constants.AdjudicatedDateValue;
                    break;
                case Constants.PropertyStatementFrom:
                    fieldName = Constants.StatementFromValue;
                    break;
                case Constants.PropertyStatementThru:
                    fieldName = Constants.StatementThruValue;
                    break;
                case Constants.LosWithCaps:
                    fieldName = Constants.PropertyLos;
                    break;
                case Constants.PropertyBillDate:
                    fieldName = Constants.BillDateValue;
                    break;
                case Constants.ClaimDate:
                    fieldName = Constants.ClaimDateValue;
                    break;
                case Constants.LastFiledDate:
                    fieldName = Constants.LastFiledDateValue;
                    break;
                case Constants.ExpectedContractualAdjustment:
                    fieldName = Constants.CalculatedAdjustment;
                    break;
                case Constants.ActualContractualAdjustment:
                    fieldName = Constants.ActualAdjustment;
                    break;
                case Constants.PropertyLinkedRemitId:
                    fieldName = Constants.LinkedRemitId;
                    break;
            }
            if (rowValue.IsNullOrWhiteSpace())
            {
                string claimColumnName = fieldName == string.Empty ? columnName : fieldName;

                object value = (claimColumnName == Constants.AdjudicatedDateValue) ? Convert.ToDateTime(claimData.AdjudicatedDateValue).ToString("MM/dd/yyyy HH:mm",

                    CultureInfo.InvariantCulture) : GetPropertyValue(claimData, claimColumnName);
                if (IsAmountColumn(claimColumnName))
                {
                    rowValue = value != null ? decimal.Parse(value.ToString()).ToString("C2") : string.Empty;
                }
                else
                {
                    rowValue = value != null ? value.ToString() : string.Empty;
                }
            }
            return rowValue;
        }

        /// <summary>
        /// Get Property Value based on property name
        /// </summary>
        /// <param name="src"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        private static object GetPropertyValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        /// <summary>
        /// prepares list of date type variables required for the report. if position of fields are altered, client header also should be altered 
        /// </summary>
        /// <param name="dateTypeRowValuesAfterLos">date type row values</param>
        /// <param name="stringBuilder">list of date type variables required for variance report</param>
        private static void GetRowValuesFromBillDateToLastFilledDate(IList<string> dateTypeRowValuesAfterLos,
            StringBuilder stringBuilder)
        {
            int finalDatesCount = dateTypeRowValuesAfterLos.Count;
            for (int dateTypeAfterLosIndex = 0;
                dateTypeAfterLosIndex < dateTypeRowValuesAfterLos.Count;
                dateTypeAfterLosIndex++)
            {
                if (dateTypeRowValuesAfterLos[dateTypeAfterLosIndex] == Constants.DateTime1900)
                    dateTypeRowValuesAfterLos[dateTypeAfterLosIndex] = null;
                if (dateTypeAfterLosIndex == finalDatesCount - 1)
                {
                    var dateTypeFinalRowValue = dateTypeRowValuesAfterLos[dateTypeAfterLosIndex];
                    if (dateTypeFinalRowValue != null)
                        stringBuilder.Append(dateTypeFinalRowValue.Replace(Constants.Comma, Constants.Space));
                    stringBuilder.Append(Constants.NewLine);
                }
                else
                    AddComma(dateTypeRowValuesAfterLos[dateTypeAfterLosIndex], stringBuilder);
            }
        }

        /// <summary>
        /// Preparing Comma separated value
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="stringBuilder">The string builder.</param>
        private static void AddComma(string value, StringBuilder stringBuilder)
        {
            if (value != null)
            {
                if (value.StartsWith(Constants.SingleQuote))
                {
                    stringBuilder = TrimEnd(stringBuilder);
                }
                stringBuilder.Append(value.Contains(Constants.Currency)
                    ? value.Replace(Constants.Comma, string.Empty)
                    : value.Replace(Constants.Comma, Constants.Space));
            }
            stringBuilder.Append(Constants.Comma + Constants.Space);
        }

        /// <summary>
        /// Preparing Comma separated value
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="columnName"></param>
        private static void AddComma(string value, StringBuilder stringBuilder, string columnName)
        {
            if ((columnName == Constants.AdjudicatedDate))
            {
                stringBuilder = TrimEnd(stringBuilder);
            }
            if (value != null)
                stringBuilder.Append(value.Contains(Constants.Currency)
                    ? value.Replace(Constants.Comma, string.Empty)
                    : value.Replace(Constants.Comma, Constants.Space));
            stringBuilder.Append(Constants.Comma + Constants.Space);
        }
        /// <summary>
        /// Trim End
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        private static StringBuilder TrimEnd(StringBuilder sb)
        {
            if (sb == null || sb.Length == 0) return sb;

            int i = sb.Length - 1;
            for (; i >= 0; i--)
                if (!char.IsWhiteSpace(sb[i]))
                    break;

            if (i < sb.Length - 1)
                sb.Length = i + 1;

            return sb;
        }
        /// <summary>
        /// prepares list of string variables required for the report. if position of fields are altered, client header also should be altered
        /// </summary>
        /// <param name="claimData">claim related data</param>
        /// <returns>list of strings type variables required for variance report</returns>
        private static IEnumerable<string> GetStringRowValuesFromClaimIdToPatientAccountNumber(ClaimDataViewModel claimData)
        {
            return new[]
            {
                claimData.ClaimId.ToString(CultureInfo.InvariantCulture),claimData.AdjudicatedContractName==null?null:claimData.AdjudicatedContractName.ToString(CultureInfo.InvariantCulture),claimData.IsReviewed?Constants.ReviewedOptionYes:Constants.ReviewedOptionNo,claimData.SsiNumber.ToString(),claimData.PatientAccountNumber
            };
        }

        /// <summary>
        /// prepares list of string variables required for the report. if position of fields are altered, client header also should be altered
        /// </summary>
        /// <param name="claimData">claim related data</param>
        /// <returns>list of strings type variables required for variance report</returns>
        private static IEnumerable<string> GetStringRowValuesFromNpiToMrn(ClaimDataViewModel claimData)
        {
            return new[]
            {
                claimData.Npi, claimData.ClaimType, claimData.PayerSequence, claimData.BillType, claimData.Drg,
                claimData.PriIcddCode, claimData.PriIcdpCode, claimData.PriPayerName.Replace(Constants.Comma, Constants.SemiColon), claimData.SecPayerName.Replace(Constants.Comma, Constants.SemiColon), claimData.TerPayerName.Replace(Constants.Comma, Constants.SemiColon),
                claimData.IsRemitLinked, claimData.ClaimStat, claimData.ClaimLink, claimData.LinkedRemitId, claimData.DischargeStatus, claimData.CustomField1, 
                claimData.CustomField2, claimData.CustomField3, claimData.CustomField4, claimData.CustomField5,
                claimData.CustomField6, claimData.MemberId,claimData.Icn,claimData.Mrn,Convert.ToString(claimData.InsuredsGroupNumber)
            };
        }

        /// <summary>
        /// prepares list of double(amount) variables required for the report. if position of fields are altered, client header also should be altered
        /// </summary>
        /// <param name="claimData">claim related data</param>
        /// <returns>list of double(amount) variables required for the report</returns>
        private static IEnumerable<double?> GetAmountRowValues(ClaimDataViewModel claimData)
        {
            return new[]
            {
                claimData.ClaimTotal, claimData.AdjudicatedValue, claimData.ActualPayment,
                claimData.PatientResponsibility, claimData.RemitAllowedAmt, claimData.RemitNonCovered, claimData.CalculatedAdjustment, claimData.ActualAdjustment, claimData.ContractualVariance, claimData.PaymentVariance
            };
        }

        /// <summary>
        /// prepares list of date type variables required for the report.if position of fields are altered, client header also should be altered
        /// </summary>
        /// <param name="claimData">claim related data</param>
        /// <returns>list of date type variables required for the report</returns>
        private static IEnumerable<string> GetDateTypeRowValues(ClaimDataViewModel claimData)
        {
            return new[]
            {
                // Gets the Adjudication DateTime based on client TimeZone on Download of Excel Sheet.
               Convert.ToDateTime(claimData.AdjudicatedDateValue).ToString("MM/dd/yyyy HH:mm",

                    CultureInfo.InvariantCulture),claimData.CheckDate,claimData.CheckNumber,claimData.StatementFromValue, claimData.StatementThruValue
            };
        }


        /// <summary>
        /// prepares list of date type variables required for the report.if position of fields are altered, client header also should be altered
        /// </summary>
        /// <param name="claimData">claim related data</param>
        /// <returns>list of date type variables required for the report</returns>
        private static IEnumerable<string> GetDateTypeRowValuesAfterLos(ClaimDataViewModel claimData)
        {
            return new[]
            {
                // Gets the BillDateValue.
                claimData.BillDateValue, claimData.ClaimDateValue, claimData.LastFiledDateValue
            };
        }

        /// <summary>
        /// prepares integer type variables required for the report.if position of fields are altered, client header also should be altered
        /// </summary>
        /// <param name="claimData">claim related data</param>
        /// <returns>list of integer type variables required for the report</returns>
        private static string GetAgeRowValues(ClaimDataViewModel claimData)
        {
            // Gets the Age.
            return claimData.Age;
        }


        /// <summary>
        /// prepares list of integer type variables required for the report.if position of fields are altered, client header also should be altered
        /// </summary>
        /// <param name="claimData">claim related data</param>
        /// <returns>list of integer type variables required for the report</returns>
        private static string GetLosRowValues(ClaimDataViewModel claimData)
        {
            // Gets the Los.
            return claimData.Los;
        }

        /// <summary>
        /// uses telerik reporting tool and writes data to either pdf or excel file according to selected report format from UI
        /// </summary>
        /// <param name="reportToExport">object of the class which prepares report</param>
        /// <param name="reportFormat">either pdf or excel format</param>
        /// <param name="varianceReportPath">html to excel template file path</param>
        /// <param name="baseFileName"></param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public static string CreateFileUsingTelerik(IReportDocument reportToExport, Enums.DownloadFileType reportFormat, string varianceReportPath, string baseFileName, string currentDateTime)
        {
            var instanceReportSource = new InstanceReportSource { ReportDocument = reportToExport };
            var result = new ReportProcessor().RenderReport(reportFormat.ToString(), instanceReportSource, null);


            string fileName = string.Format("{0}{1}.{2}", baseFileName, currentDateTime, result.Extension);

            string filePath = Path.Combine(varianceReportPath, fileName);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
            }
            return fileName;
        }


    }
}