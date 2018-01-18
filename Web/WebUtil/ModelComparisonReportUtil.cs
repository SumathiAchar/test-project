using System.Globalization;
using Microsoft.Ajax.Utilities;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SSI.ContractManagement.Web.WebUtil
{
    public class ModelComparisonReportUtil
    {
        /// <summary>
        /// Gets the name of the exported file.
        /// </summary>
        /// <param name="modelComparisonReportList">The model comparison report list.</param>
        /// <param name="configuredPath">The configured path.</param>
        /// <returns></returns>
        public string GetExportedFileName(ModelComparisonReportViewModel modelComparisonReportList, string configuredPath)
        {
            if (!Directory.Exists(configuredPath))
                Directory.CreateDirectory(configuredPath);
            //FIXED-JITEN-JUNE - Why to convert Constants.EmptyReportResult to ToString. This can be a string in the constant file itself. 
            //Manual conversion can be avoided in this way
            return modelComparisonReportList.ModelComparisonData.Count == 0
                ? Constants.EmptyReportResult
                : GenerateCsvFile(modelComparisonReportList.ModelComparisonData, modelComparisonReportList.IsCheckedDetailLevel);
        }

        /// <summary>
        /// Generates the CSV file.
        /// </summary>
        /// <param name="modelComparisonDataList">The list.</param>
        /// <param name="detailLevel">if set to <c>true</c> [detail level].</param>
        /// <returns></returns>
        //REVIEW-JITEN-JUNE - This method is too big and difficult to understand
        //Do not use string literals(\\, C2 etc). local variables can not be called as i, j
        //simplify and segregate this method into 2 methods each for detailed and expanded level.
        //Put simple and clear logic instead of tedious loops
        private string GenerateCsvFile(List<ModelComparisonReportDetails> modelComparisonDataList, bool detailLevel)
        {
            string dateTimeStamp = DateTime.Now.ToString(Constants.DateTimeExtendedFormat);
            string stamp = string.Format("{0}{1}.{2}", Constants.ModelComparisonReportFileBaseName, dateTimeStamp,
                 Enums.DownloadFileType.Csv);
            string fileName = GlobalConfigVariable.ReportsFilePath + Constants.FileNameSeperator + stamp;
            if (!File.Exists(fileName))
            {
                using (var writer = new StreamWriter(fileName))
                {
                    var stringBuilder = new StringBuilder();
                    writer.WriteLine(Constants.ModelComparisonHeader);
                    if (!detailLevel)
                    {
                        foreach (var modelComparisonData in modelComparisonDataList)
                        {
                            IEnumerable<string> stringRowValues = GetStringRowValues(modelComparisonData);
                            IList<double?> amountRowValues = GetAmountRowValues(modelComparisonData);

                            foreach (var rowValue in stringRowValues)
                            {
                                AddComma(rowValue, stringBuilder);
                            }
                            int count = amountRowValues.Count;
                            for (int index = 0; index < amountRowValues.Count; index++)
                            {
                                if (index == count - 1)
                                {
                                    var amountRowValue = amountRowValues[index];
                                    if (amountRowValue != null)
                                        stringBuilder.Append(Convert.ToDecimal(amountRowValue).ToString(Constants.AmountFormat).Replace(Constants.Comma, string.Empty));
                                    stringBuilder.Append(Constants.NewLine);
                                    stringBuilder.Append(Constants.NewLine);
                                }
                                else
                                {
                                    var amountRowValue = amountRowValues[index];
                                    string amountRow = amountRowValue.ToString();
                                    AddComma(string.IsNullOrEmpty(amountRow)
                                        ? string.Empty
                                        : Convert.ToDecimal(amountRow).ToString(Constants.AmountFormat), stringBuilder);
                                }
                            }
                        }

                    }
                    else
                    {
                        foreach (var modelComparisonData in modelComparisonDataList.DistinctBy(a => a.ClaimData.ModelId))
                        {
                            var modelComparisonList = modelComparisonDataList.Select(x => x)
                                .Where(y => y.ClaimData.ModelId == modelComparisonData.ClaimData.ModelId).ToList();
                            AddComma(modelComparisonData.ClaimData.NodeText, stringBuilder);
                            stringBuilder.Append(Constants.NewLine);
                            foreach (var modelComparisonReportDetails in modelComparisonList)
                            {
                                IList<double?> amountRowValues = GetAmountRowValues(modelComparisonReportDetails);

                                AddComma(string.IsNullOrEmpty(modelComparisonReportDetails.ClaimData.DetailedSelection) ? Constants.NoValue : modelComparisonReportDetails.ClaimData.DetailedSelection, stringBuilder);
                                AddComma(modelComparisonReportDetails.Count, stringBuilder);

                                int count = amountRowValues.Count;
                                for (int index = 0; index < amountRowValues.Count; index++)
                                {
                                    if (index == count - 1)
                                    {
                                        var amountRowValue = amountRowValues[index];
                                        if (amountRowValue != null)
                                            stringBuilder.Append(Convert.ToDecimal(amountRowValue).ToString(Constants.AmountFormat).Replace(Constants.Comma, string.Empty));
                                        stringBuilder.Append(Constants.NewLine);
                                    }
                                    else
                                    {
                                        var amountRowValue = amountRowValues[index];
                                        string amountRow = amountRowValue.ToString();
                                        AddComma(string.IsNullOrEmpty(amountRow)
                                            ? string.Empty
                                            : Convert.ToDecimal(amountRow).ToString(Constants.AmountFormat), stringBuilder);
                                    }
                                }
                            }
                            stringBuilder.Append(Constants.Total);
                            AddComma(null, stringBuilder);
                            AddComma(modelComparisonList.Sum(x => Convert.ToDecimal(x.Count)).ToString(CultureInfo.InvariantCulture), stringBuilder);
                            AddComma(
                                string.IsNullOrEmpty(modelComparisonList.Sum(x => x.ClaimData.ClaimTotal).ToString())
                                    ? string.Empty
                                    : Convert.ToDecimal(modelComparisonList.Sum(x => x.ClaimData.ClaimTotal).ToString())
                                        .ToString(Constants.AmountFormat), stringBuilder);

                            AddComma(
                                string.IsNullOrEmpty(
                                    modelComparisonList.Sum(x => x.ClaimData.AdjudicatedValue).ToString())
                                    ? string.Empty
                                    : Convert.ToDecimal(
                                        modelComparisonList.Sum(x => x.ClaimData.AdjudicatedValue).ToString())
                                        .ToString(Constants.AmountFormat), stringBuilder);

                            AddComma(
                                string.IsNullOrEmpty(modelComparisonList.Sum(x => x.ClaimData.ActualPayment).ToString())
                                    ? string.Empty
                                    : Convert.ToDecimal(
                                        modelComparisonList.Sum(x => x.ClaimData.ActualPayment).ToString())
                                        .ToString(Constants.AmountFormat), stringBuilder);

                            AddComma(
                                string.IsNullOrEmpty(
                                    modelComparisonList.Sum(x => x.ClaimData.PatientResponsibility).ToString())
                                    ? string.Empty
                                    : Convert.ToDecimal(
                                        modelComparisonList.Sum(x => x.ClaimData.PatientResponsibility).ToString())
                                        .ToString(Constants.AmountFormat), stringBuilder);

                            AddComma(
                                string.IsNullOrEmpty(
                                    modelComparisonList.Sum(x => x.ClaimData.RemitAllowedAmt).ToString())
                                    ? string.Empty
                                    : Convert.ToDecimal(
                                        modelComparisonList.Sum(x => x.ClaimData.RemitAllowedAmt).ToString())
                                        .ToString(Constants.AmountFormat), stringBuilder);

                            AddComma(
                                string.IsNullOrEmpty(
                                    modelComparisonList.Sum(x => x.ClaimData.RemitNonCovered).ToString())
                                    ? string.Empty
                                    : Convert.ToDecimal(
                                        modelComparisonList.Sum(x => x.ClaimData.RemitNonCovered).ToString())
                                        .ToString(Constants.AmountFormat), stringBuilder);

                            AddComma(
                                string.IsNullOrEmpty(
                                    modelComparisonList.Sum(x => x.ClaimData.CalculatedAdjustment).ToString())
                                    ? string.Empty
                                    : Convert.ToDecimal(
                                        modelComparisonList.Sum(x => x.ClaimData.CalculatedAdjustment).ToString())
                                        .ToString(Constants.AmountFormat), stringBuilder);

                            AddComma(
                                string.IsNullOrEmpty(
                                    modelComparisonList.Sum(x => x.ClaimData.ActualAdjustment).ToString())
                                    ? string.Empty
                                    : Convert.ToDecimal(
                                        modelComparisonList.Sum(x => x.ClaimData.ActualAdjustment).ToString())
                                        .ToString(Constants.AmountFormat), stringBuilder);

                            AddComma(
                                string.IsNullOrEmpty(
                                    modelComparisonList.Sum(x => x.ClaimData.ContractualVariance).ToString())
                                    ? string.Empty
                                    : Convert.ToDecimal(
                                        modelComparisonList.Sum(x => x.ClaimData.ContractualVariance).ToString())
                                        .ToString(Constants.AmountFormat), stringBuilder);

                            stringBuilder.Append(string.IsNullOrEmpty(
                                modelComparisonList.Sum(x => x.ClaimData.PaymentVariance).ToString())
                                ? string.Empty
                                : Convert.ToDecimal(
                                    modelComparisonList.Sum(x => x.ClaimData.PaymentVariance).ToString())
                                    .ToString(Constants.AmountFormat).Replace(Constants.Comma, string.Empty));

                            stringBuilder.Append(Constants.NewLine);
                            stringBuilder.Append(Constants.NewLine);
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
        /// <param name="claimData">The claim data.</param>
        /// <returns></returns>
        private static IEnumerable<string> GetStringRowValues(ModelComparisonReportDetails claimData)
        {
            return new[]
            {
                claimData.ClaimData.NodeText, claimData.Count 
            };
        }

        /// <summary>
        /// Gets the amount row values.
        /// </summary>
        /// <param name="claimData">The claim data.</param>
        /// <returns></returns>
        private static IList<double?> GetAmountRowValues(ModelComparisonReportDetails claimData)
        {
            return new[]
            {
                claimData.ClaimData.ClaimTotal, claimData.ClaimData.AdjudicatedValue, claimData.ClaimData.ActualPayment,
                claimData.ClaimData.PatientResponsibility, claimData.ClaimData.RemitAllowedAmt, claimData.ClaimData.RemitNonCovered, claimData.ClaimData.CalculatedAdjustment, claimData.ClaimData.ActualAdjustment, claimData.ClaimData.ContractualVariance, claimData.ClaimData.PaymentVariance
            };
        }
    }
}