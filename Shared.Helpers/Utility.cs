using ExpressionEvaluator;
using Microsoft.VisualBasic.FileIO;
using SSI.ContractManagement.Shared.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SSI.ContractManagement.Shared.Helpers
{
    public class Utility
    {
        /// <summary>
        /// Pads the left to code.
        /// </summary>
        /// <param name="claimCodes">The claim codes.</param>
        /// <param name="conditionRange">The condition range.</param>
        public void PadLeftToCode(List<string> claimCodes, ConditionRange conditionRange)
        {
            if (claimCodes != null && claimCodes.Count > 0 && conditionRange != null)
            {
                int maxLength = Math.Max(GetMaxLength(conditionRange), claimCodes.Max(claimCode => claimCode.Length));

                //Pads left with spaces to make all items of same size
                conditionRange.StartValue = conditionRange.StartValue.PadLeft(maxLength);
                if (conditionRange.EndValue != null)
                    conditionRange.EndValue = conditionRange.EndValue.PadLeft(maxLength);

                //Pads left with spaces to make all items of same size
                for (int claimCodesIndex = 0; claimCodesIndex < claimCodes.Count; claimCodesIndex++)
                    claimCodes[claimCodesIndex] = claimCodes[claimCodesIndex].PadLeft(maxLength);
            }
        }

        /// <summary>
        /// Gets the maximum length.
        /// </summary>
        /// <param name="contractCode">The contract codes.</param>
        /// <returns></returns>
        private int GetMaxLength(ConditionRange contractCode)
        {
            int maxLength = 0;
            if (contractCode != null)
            {
                maxLength = Math.Max(contractCode.StartValue.Length,
                    contractCode.EndValue == null ? 0 : contractCode.EndValue.Length);
            }
            return maxLength;
        }

        /// <summary>
        /// Gets the ranges. Based on comma,* and - we are defining Ranges .
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public List<ConditionRange> GetRanges(string code)
        {
            List<string> codeList = code.Split(',').ToList();
            List<ConditionRange> listOfString = new List<ConditionRange>();

            foreach (var list in codeList)
            {
                //"-" represents range values Ex:Revcode = 350-450
                List<string> rangeList = new List<string>(list.Split(':'));
                if (rangeList.Count == 2)
                {
                    listOfString.Add(new ConditionRange
                    {
                        StartValue = rangeList[0].ToLower(),
                        EndValue = rangeList[1].ToLower()
                    });
                }
                else
                {
                    listOfString.Add(new ConditionRange { StartValue = list.ToLower(), EndValue = null });
                }
            }
            return listOfString;
        }

        /// <summary>
        /// Gets the name of the ranges for primary payer.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public IEnumerable<ConditionRange> GetRangesForPriPayerName(string code)
        {
            List<string> codeList = code.Split(';').ToList();
            List<ConditionRange> listOfString = new List<ConditionRange>();

            foreach (var list in codeList)
            {
                //"-" represents range values Ex:Revcode = 350-450
                List<string> rangeList = new List<string>(list.Split(':'));
                if (rangeList.Count == 2)
                {
                    listOfString.Add(new ConditionRange
                    {
                        StartValue = rangeList[0].ToLower(),
                        EndValue = rangeList[1].ToLower()
                    });
                }
                else
                {
                    listOfString.Add(new ConditionRange { StartValue = list.ToLower(), EndValue = null });
                }
            }
            return listOfString;
        }

        /// <summary>
        /// Determines whether [is valid length] [the specified contract codes].
        /// </summary>
        /// <param name="contractCodes">The contract codes.</param>
        /// <param name="claimCodes">The claim codes.</param>
        /// <param name="conditionOperation">The condition operation.</param>
        /// <returns></returns>
        public bool IsValidLength(ConditionRange contractCodes, IEnumerable<string> claimCodes,
            Enums.ConditionOperation conditionOperation)
        {
            if (claimCodes != null)
                //Match claim code's length with maximum of all contract codes
                return (from item in claimCodes
                        let length = GetMaxLength(contractCodes)
                        where
                            conditionOperation == Enums.ConditionOperation.GreaterThan && item.Length >= length ||
                            conditionOperation == Enums.ConditionOperation.LessThan && item.Length <= length
                        select item).Any();
            return false;
        }

        /// <summary>
        /// Pads the condition ranges.
        /// </summary>
        /// <param name="conditionRange">The condition range.</param>
        /// <returns></returns>
        public ConditionRange PadConditionRange(ConditionRange conditionRange)
        {
            if (conditionRange != null)
            {
                //Get Max length from condition ranges
                int maxlength = GetMaxLength(conditionRange);
                //Pads left with spaces to make all items of same size
                if (conditionRange.StartValue != null)
                    conditionRange.StartValue = conditionRange.StartValue.PadLeft(maxlength);
                if (conditionRange.EndValue != null)
                    conditionRange.EndValue = conditionRange.EndValue.PadLeft(maxlength);
            }
            return conditionRange;
        }

        /// <summary>
        /// Resets the payment result.
        /// </summary>
        /// <param name="paymentResult">The payment result.</param>
        public void ResetPaymentResult(PaymentResult paymentResult)
        {
            paymentResult.AdjudicatedValue = null;
            paymentResult.ServiceTypeId = null;
            paymentResult.PaymentTypeDetailId = null;
            paymentResult.PaymentTypeId = null;
            paymentResult.IsInitialEntry = false;
            paymentResult.ClaimStatus = (byte)Enums.AdjudicationOrVarianceStatuses.UnAdjudicated;
        }

        /// <summary>
        /// Updates the payment result.
        /// </summary>
        /// <param name="paymentResult">The payment result.</param>
        /// <param name="serviceTypeId">The service type identifier.</param>
        /// <param name="paymentTypeDetailId">The payment type detail identifier.</param>
        /// <param name="paymentTypeId">The payment type identifier.</param>
        public void UpdatePaymentResult(PaymentResult paymentResult, long? serviceTypeId,
            long? paymentTypeDetailId, int? paymentTypeId)
        {
            paymentResult.ServiceTypeId = serviceTypeId;
            paymentResult.PaymentTypeDetailId = paymentTypeDetailId;
            paymentResult.PaymentTypeId = paymentTypeId;
            paymentResult.MicrodynEditErrorCodes = null;
            paymentResult.MicrodynEditReturnRemarks = null;
            paymentResult.MicrodynPricerErrorCodes = null;
        }

        /// <summary>
        /// Determines whether [is condition not valid] [the specified is valid].
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        /// <param name="conditionOperation">The condition operation.</param>
        /// <returns></returns>
        public bool IsConditionNotValid(bool isValid, Enums.ConditionOperation conditionOperation)
        {
            return (conditionOperation == Enums.ConditionOperation.Contains
                ? (!isValid && conditionOperation != Enums.ConditionOperation.Contains)
                : ((!isValid && conditionOperation == Enums.ConditionOperation.NotEqualTo) ||
                   (isValid && conditionOperation != Enums.ConditionOperation.NotEqualTo &&
                    conditionOperation != Enums.ConditionOperation.Contains)));
        }


        /// <summary>
        /// Gets the list of codes.
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <returns></returns>
        private long GetListOfCodes(string codes)
        {
            long totalCount = 0;
            if (!String.IsNullOrEmpty(codes))
            {
                if (codes.Contains(","))
                {
                    List<string> splittedString = new List<string>(codes.Split(','));
                    foreach (string tempCode in splittedString)
                    {
                        if (tempCode.Contains(":"))
                            totalCount += GetCodesFromRangeString(tempCode);
                        else if (tempCode.Contains("*"))
                            totalCount += 9;
                        else
                            totalCount += 1;
                    }
                }
                else
                {
                    if (codes.Contains(":"))
                        totalCount += GetCodesFromRangeString(codes);
                    else if (codes.Contains("*"))
                        totalCount += 9;
                    else
                        totalCount += 1;
                }

                return totalCount;
            }
            return 0;
        }

        /// <summary>
        /// Gets the codes from range string.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        private long GetCodesFromRangeString(string code)
        {
            List<string> rangeCodeList = new List<string>();
            //Checking if the string code is empty or null
            if (!String.IsNullOrEmpty(code) && !String.IsNullOrWhiteSpace(code))
            {
                string[] splittedText = code.Split(':');
                string fromValue = splittedText[0];
                string toValue = splittedText[1];
                string maxValueString = "";

                long stratingValue;
                long endingValue;
                bool fromValueIsNumber = Int64.TryParse(fromValue, out stratingValue);
                bool toValueIsNumber = Int64.TryParse(toValue, out endingValue);
                //Checking if string code contains integer value or not
                if (fromValueIsNumber && toValueIsNumber)
                {
                    for (long i = stratingValue; i <= endingValue; i++)
                    {
                        rangeCodeList.Add(i.ToString(CultureInfo.InvariantCulture).PadLeft(toValue.Length, '0'));
                    }
                }
                else
                {
                    int range = 0;
                    int starting = 0;
                    if (fromValue.Length == toValue.Length)
                    {
                        char st = fromValue[0];
                        char lt = toValue[0];

                        if (st >= 65 && st <= 90 && lt >= 65 && lt <= 90)
                        {
                            if (st <= lt)
                            {
                                string startSubstring = fromValue.Substring(1, fromValue.Length - 1);
                                string endSubstring = toValue.Substring(1, toValue.Length - 1);
                                int startSubstringNumber;
                                int endSubstringNumber;
                                bool isStartcontainsNumber = Int32.TryParse(startSubstring, out startSubstringNumber);
                                bool isEndcontainsNumber = Int32.TryParse(endSubstring, out endSubstringNumber);

                                if (isStartcontainsNumber && isEndcontainsNumber)
                                {
                                    string sartingRangeWithZeros;
                                    if (fromValue.Substring(1, 1) == "0")
                                    {
                                        sartingRangeWithZeros = fromValue[0] + "0";
                                        for (int i = 2; i < fromValue.Length; i++)
                                        {
                                            if (fromValue.Substring(i, 1) == "0")
                                            {
                                                sartingRangeWithZeros += "0";
                                            }
                                            else
                                            {
                                                break;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        sartingRangeWithZeros = fromValue[0] + "";
                                    }


                                    string endRangeWithZeros;
                                    if (toValue.Substring(1, 1) == "0")
                                    {
                                        endRangeWithZeros = toValue[0] + "0";
                                        for (int i = 2; i < toValue.Length; i++)
                                        {
                                            if (toValue.Substring(i, 1) == "0")
                                            {
                                                endRangeWithZeros += "0";
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        endRangeWithZeros = toValue[0] + "";
                                    }

                                    int diff2 = toValue.Length - endRangeWithZeros.Length;
                                    string endRangeNumericValue = toValue.Substring(toValue.Length - diff2, diff2);
                                    if (endRangeNumericValue != "")
                                    {
                                        range = Convert.ToInt32(endRangeNumericValue);
                                    }
                                    int difference = fromValue.Length - sartingRangeWithZeros.Length;
                                    string startRangeNumericValue = fromValue.Substring(fromValue.Length - difference,
                                        difference);

                                    if (startRangeNumericValue != "")
                                    {
                                        starting = Convert.ToInt32(startRangeNumericValue);
                                    }

                                    int digitCount = fromValue.Length;
                                    for (int i = 1; i <= digitCount - 1; i++)
                                    {
                                        maxValueString += "9";
                                    }
                                    Int64 maxDigit = Convert.ToInt64(maxValueString);

                                    while (st <= lt)
                                    {
                                        int diff;
                                        if (st != lt)
                                        {
                                            for (int i = starting; i <= maxDigit; i++)
                                            {
                                                string s = sartingRangeWithZeros + "" + i;
                                                if (s.Length == fromValue.Length)
                                                {
                                                    rangeCodeList.Add(s);
                                                }
                                                else
                                                {
                                                    diff = s.Length - fromValue.Length;
                                                    if (diff > 0)
                                                    {
                                                        int zeroIndex = s.IndexOf('0');
                                                        s = s.Remove(zeroIndex, diff);
                                                    }
                                                    rangeCodeList.Add(s);
                                                }
                                            }
                                            char newChar = ++st;
                                            string zeros = "";
                                            if (digitCount >= 3)
                                            {
                                                for (int i = 1; i <= digitCount - 2; i++)
                                                {
                                                    zeros += "0";
                                                }
                                            }
                                            sartingRangeWithZeros = newChar + zeros;
                                            starting = 0;
                                        }
                                        else
                                        {
                                            for (int i = starting; i <= range; i++)
                                            {
                                                string s = sartingRangeWithZeros + "" + i;
                                                if (s.Length == fromValue.Length)
                                                {
                                                    rangeCodeList.Add(s);
                                                }
                                                else
                                                {
                                                    diff = s.Length - fromValue.Length;
                                                    if (diff > 0)
                                                    {
                                                        int zeroIndex = s.IndexOf('0');
                                                        s = s.Remove(zeroIndex, diff);
                                                    }
                                                    rangeCodeList.Add(s);
                                                }
                                            }
                                            ++st;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return rangeCodeList.Count;
        }

        /// <summary>
        /// Check Selected Search criteria is too large or not for Request Adjudication as well as for Reports
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public bool IsTooManySelectionForClaimFields(string searchCriteria)
        {
            bool isLargeRange = false;

            List<string> tempClaimField = new List<string>();

            List<ContractServiceLineClaimFieldSelection> contractServiceLineClaimFieldSelectionList = null;
            if (!String.IsNullOrEmpty(searchCriteria))
            {
                if (searchCriteria.Contains("~"))
                {
                    tempClaimField = (searchCriteria.Split('~').ToList());
                }
                else
                {
                    tempClaimField.Add(searchCriteria);
                }
            }

            if (tempClaimField.Count > 0)
            {
                contractServiceLineClaimFieldSelectionList =
                    tempClaimField.Select(singleClaim => singleClaim.Split('|').ToList()).Select(
                        tempClaim => new ContractServiceLineClaimFieldSelection
                        {
                            ClaimFieldId = Convert.ToInt64(tempClaim[0]),
                            Operator = Convert.ToInt32(tempClaim[1]),
                            Values = Convert.ToString(tempClaim[2])
                        }).ToList();
            }

            long totalCount = 0;

            if (contractServiceLineClaimFieldSelectionList != null &&
                contractServiceLineClaimFieldSelectionList.Count > 0)
            {
                totalCount +=
                    contractServiceLineClaimFieldSelectionList.Sum(
                        contractServiceLineClaimFieldSelection =>
                            GetListOfCodes(contractServiceLineClaimFieldSelection.Values));
            }

            if (totalCount > GlobalConfigVariable.MaxClaimFieldSelectionCount)
                isLargeRange = true;

            return isLargeRange;
        }

        /// <summary>
        /// Gets the current local time string.
        /// </summary>
        /// <param name="currentDateTime">The time zone information.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public string GetLocalTimeString(string currentDateTime, DateTime? dateTime = null)
        {
            if (dateTime == null)
            {
                DateTime parsedDateTime;
                if (!string.IsNullOrEmpty(currentDateTime) &&
                    DateTime.TryParseExact(currentDateTime, Constants.DateTimeSimpleFormat, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out parsedDateTime))
                    return parsedDateTime.ToString(CultureInfo.InvariantCulture);

                return DateTime.Now.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                DateTime parsedDateTime;
                if (!(!string.IsNullOrEmpty(currentDateTime) &&
                      DateTime.TryParseExact(currentDateTime, Constants.DateTimeSimpleFormat,
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out parsedDateTime)))
                    parsedDateTime = DateTime.UtcNow;

                return
                    Convert.ToDateTime(dateTime)
                        .AddTicks((parsedDateTime - DateTime.UtcNow).Ticks)
                        .ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Validates the expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="claimValuePair">The claim value pair.</param>
        /// <returns></returns>
        public bool ValidateExpression(string expression, Dictionary<string, string> claimValuePair)
        {
            var variableFields =
                expression.ToUpper(CultureInfo.InvariantCulture)
                    .Split(Constants.AllowedOperators)
                    .Where(item => item.Trim() != string.Empty)
                    .Select(field => field.Trim());

            double amount;
            //If the variable fields is present in claimValuePair, the expression is evaluated
            if (
                !variableFields.Any(
                    item => !claimValuePair.ContainsKey(item) && double.TryParse(item, out amount) == false))
            {
                bool isValid = true;
                //Ignoring the If condition to replace in the string before the string is evaluated
                claimValuePair.Remove(Constants.If);
                string expandedExpression =
                    claimValuePair.Aggregate(expression.ToUpper(CultureInfo.InvariantCulture),
                        (result, pair) => Regex.Replace(result, pair.Key, pair.Value, RegexOptions.IgnoreCase));
                expandedExpression = Regex.Replace(expandedExpression, Constants.MathMax, Constants.GreaterOf,
                    RegexOptions.IgnoreCase);
                expandedExpression = Regex.Replace(expandedExpression, Constants.MathMin, Constants.LesserOf,
                    RegexOptions.IgnoreCase);
                claimValuePair.Add(Constants.If, Constants.MathMin);
                try
                {
                    var result = new Expression(expandedExpression).Evaluate();
                    double output;
                    if (double.TryParse(result.ToString(), out output))
                    {
                        //Calculates the threshold based on the expression
                        var threshold = Math.Round(Convert.ToDouble(result), Constants.Two);
                        //If the expression results Infinity or Not a Number, returns invalid formula 
                        if (double.IsInfinity(threshold) || double.IsNaN(threshold))
                        {
                            isValid = false;
                        }
                    }
                    // If expression output is not a double value, returns Invalid formula
                    else
                    {
                        isValid = false;
                    }
                }

                catch (Exception)
                {
                    isValid = false;
                }
                return isValid;
            }
            return false;
        }

        /// <summary>
        /// Evaluates the expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="paymentTypeBase">The payment type base.</param>
        /// <returns></returns>
        public double EvaluateExpression(string expression, IEvaluateableClaim claim,
            PaymentTypeBase paymentTypeBase)
        {
            double threshold = 1;
            paymentTypeBase.ExpandedExpression = Constants.ThresholdSmartBoxVariables.Where(
                s => claim.SmartBox.GetType().GetProperty(s) != null)
                .Aggregate(expression, (current, s) =>
                    current.Replace(s,
                        Convert.ToString(claim.SmartBox.GetType().GetProperty(s).GetValue(claim.SmartBox, null))));
            try
            {
                var expresult = new Expression(paymentTypeBase.ExpandedExpression).Evaluate();
                double output;
                if (double.TryParse(expresult.ToString(), out output))
                {

                    threshold = Math.Round(Convert.ToDouble(expresult), 4);

                    if (double.IsInfinity(threshold) || double.IsNaN(threshold))
                    {
                        threshold = 1;
                        paymentTypeBase.IsFormulaError = true;
                    }
                }
                else
                {
                    threshold = 1;
                    paymentTypeBase.IsFormulaError = true;
                }
            }
            catch (Exception)
            {
                paymentTypeBase.IsFormulaError = true;
            }

            return threshold;
        }


        /// <summary>
        /// Calculates the allowed amount.
        /// </summary>
        /// <param name="paymentResults">The payment results.</param>
        /// <returns></returns>
        public double? CalculateAllowedAmount(IEnumerable<PaymentResult> paymentResults)
        {
            return Convert.ToDouble(paymentResults.Sum(x => x.AdjudicatedValue));
        }

        /// <summary>
        /// updating stoploss details to payment result
        /// </summary>
        /// <param name="paymentResult">payment result</param>
        /// <param name="smartBox">user added variables</param>
        /// <param name="paymentTypeStopLoss">user added stop loss details</param>
        /// <param name="isThresholdFormulaError">if there are any threshold formula error</param>
        public void UpdateStopLossDetails(PaymentResult paymentResult, SmartBox smartBox,
            PaymentTypeStopLoss paymentTypeStopLoss, bool isThresholdFormulaError)
        {
            paymentResult.IntermediateAdjudicatedValue = smartBox.CAA;
            paymentResult.ExpressionResult = paymentTypeStopLoss.Threshold;
            if (paymentTypeStopLoss.Expression != null)
            {
                paymentResult.IsFormulaError = paymentResult.IsFormulaError || isThresholdFormulaError;
            }
            paymentResult.Expression = string.Format("{0}{1}{2}", Constants.StartSquareBracket,
                paymentTypeStopLoss.Expression, Constants.EndSquareBracket);
            paymentResult.ExpandedExpression = paymentTypeStopLoss.ExpandedExpression;
        }

        /// <summary>
        /// updating custom payment result details 
        /// </summary>
        /// <param name="claimPaymentResult">payment result to be updated</param>
        /// <param name="smartBox">user entered smart box variables</param>
        /// <param name="paymentTypeBase">user entered custom payment details</param>
        public void UpdateCustomTableDetails(PaymentResult claimPaymentResult, SmartBox smartBox,
            PaymentTypeCustomTable paymentTypeBase)
        {
            claimPaymentResult.IntermediateAdjudicatedValue = smartBox.CAA;
            claimPaymentResult.CustomExpressionResult = claimPaymentResult.AdjudicatedValue;
            claimPaymentResult.IsFormulaError = paymentTypeBase.IsFormulaError;
            claimPaymentResult.CustomExpression = string.Format("{0}{1}{2}", Constants.StartSquareBracket,
                paymentTypeBase.Expression, Constants.EndSquareBracket);
            claimPaymentResult.CustomExpandedExpression = paymentTypeBase.ExpandedExpression;
            paymentTypeBase.IsFormulaError = false;
        }

        /// <summary>
        /// used to split csv to array. It uses VB textfieldparser class which internally handles all complexities for this conversion
        /// </summary>
        /// <param name="input">csv string</param>
        /// <returns>a string array</returns>
        public List<string> SplitCsvRowToArray(string input)
        {
            string[] parts = null;
            if (!string.IsNullOrEmpty(input))
            {
                using (TextFieldParser parser = new TextFieldParser(new MemoryStream(Encoding.UTF8.GetBytes(input))))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.Delimiters = new[] { Constants.Comma };
                    parser.HasFieldsEnclosedInQuotes = false;
                    parts = parser.ReadFields();
                }
            }
            return parts != null ? parts.ToList() : new List<string>();
        }


        /// <summary>
        /// used to split csv to array. It uses VB textfieldparser class which internally handles all complexities for this conversion
        /// </summary>
        /// <param name="input">csv string</param>
        /// <returns>a string array</returns>
        public List<string> SplitCsvRowToArrayForAdjudication(string input)
        {
            string[] parts = null;
            if (!string.IsNullOrEmpty(input))
            {
                using (TextFieldParser parser = new TextFieldParser(new MemoryStream(Encoding.UTF8.GetBytes(input))))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.Delimiters = new[] { Constants.Comma };
                    parts = parser.ReadFields();
                }
            }
            return parts != null ? parts.ToList() : new List<string>();
        }

        /// <summary>
        /// converts a list of objects to data table
        /// </summary>
        /// <typeparam name="TSource">generic object</typeparam>
        /// <param name="data">data list</param>
        /// <returns>data table</returns>
        public DataTable ToDataTable<TSource>(IEnumerable<TSource> data)
        {
            DataTable dataTable = new DataTable();
            PropertyInfo[] props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                                                 prop.PropertyType);
            }

            foreach (TSource item in data)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        /// <summary>
        /// replace function with null check
        /// </summary>
        /// <param name="input">input string</param>
        /// <param name="replaceTo">word to be replace</param>
        /// <param name="replaceWith">this will be used to replace</param>
        /// <returns>new string</returns>
        public string Replace(string input, string replaceTo, string replaceWith)
        {
            return input != null ? input.Replace(replaceTo, replaceWith) : null;
        }

        /// <summary>
        /// Gets the selection criteria.
        /// </summary>
        /// <param name="selectionCriteria">The selection criteria.</param>
        /// <returns></returns>
        public string GetSelectionCreteria(string selectionCriteria)
        {
            if (!string.IsNullOrEmpty(selectionCriteria))
            {
                string[] separatedCriteria = selectionCriteria.Split(Constants.CreteriaSeparatorChar);
                if (separatedCriteria.Length != 1)
                {
                    for (var index = 0; index < separatedCriteria.Length; index++)
                    {
                        var separatedCondition = separatedCriteria[index].Split(Constants.ConditionSeparatorChar);
                        separatedCondition[2] = separatedCondition[2].Replace(Constants.ReplaceValue,
                            Constants.ActualValue);
                        separatedCriteria[index] = string.Join(Constants.ConditionSeparatorString, separatedCondition);
                    }
                    selectionCriteria = string.Join(Constants.CreteriaSeparatorString, separatedCriteria);
                }
                else
                {
                    string[] separatedCondition = separatedCriteria[0].Split(Constants.ConditionSeparatorChar);
                    separatedCondition[2] = separatedCondition[2].Replace(Constants.ReplaceValue, Constants.ActualValue);
                    selectionCriteria = string.Join(Constants.ConditionSeparatorString, separatedCondition);

                }
            }
            return selectionCriteria;
        }

        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="recipient"></param>
        public void SendMail(string subject, string body, string recipient)
        {
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings[Constants.SmtpServer],
                Convert.ToInt16(ConfigurationManager.AppSettings[Constants.SmtpPort]))
            {
                Credentials =
                    new System.Net.NetworkCredential(ConfigurationManager.AppSettings[Constants.MailFrom],
                        ConfigurationManager.AppSettings[Constants.SmtpPassword])
            };
            mail.IsBodyHtml = true;
            mail.From = new MailAddress(ConfigurationManager.AppSettings[Constants.MailFrom]);
            mail.To.Add(recipient);
            mail.Subject = subject;
            mail.Body = body;
            // ReSharper disable once AccessToStaticMemberViaDerivedType
            mail.BodyEncoding = UTF8Encoding.UTF8;
            client.Send(mail);
        }
    }
}
