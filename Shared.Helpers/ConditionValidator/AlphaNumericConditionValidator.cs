using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Helpers.ConditionValidator
{
    /// <summary>
    /// Class ValidateAlphaNumericCodes.
    /// </summary>
    public class AlphaNumericConditionValidator : IConditionValidator
    {
        /// <summary>
        /// Determines whether the specified condition is valid.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns><c>true</c> if the specified condition is valid; otherwise, <c>false</c>.</returns>
        public bool IsValid(ICondition condition)
        {
            return IsAlphanumericCodes(condition);
        }

        /// <summary>
        /// Determines whether [is alphanumeric codes] [the specified condition].
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        private bool IsAlphanumericCodes(ICondition condition)
        {
            bool isValid = false;
            bool isHcpcsCode=false; 
            //Update Code if Condition is set as Include Modifier
            if (condition.PropertyColumnName == Constants.PropertyHcpcsCodeWithModifier)
            {
                condition.LeftOperands = UpdateCodeWithIncludeModifier(condition.LeftOperands);
                condition.RightOperand = string.Join(Constants.Comma,
                    UpdateCodeWithIncludeModifier(new List<string> { condition.RightOperand }));
                isHcpcsCode = true;
            }
            if (condition.PropertyColumnName == Constants.PropertyPriPayerName)
            {
                foreach (ConditionRange conditionRange in Utilities.GetRangesForPriPayerName(condition.RightOperand))
                {
                    Enums.ConditionOperation conditionOperation = (Enums.ConditionOperation)condition.ConditionOperator;

                    //Check length is valid or not
                    isValid = Utilities.IsValidLength(conditionRange, condition.LeftOperands, conditionOperation);

                    if (!(!isValid && (conditionOperation == Enums.ConditionOperation.GreaterThan
                                       || conditionOperation == Enums.ConditionOperation.LessThan)))
                    {
                        //Pads left with spaces to make all items of same size
                        ConditionRange padConditionRange = Utilities.PadConditionRange(conditionRange);
                        Utilities.PadLeftToCode(condition.LeftOperands, padConditionRange);
                        isValid = IsAlphaNumericCodesExists(padConditionRange, condition,
                            conditionOperation);
                    }

                    //Break loop is condition is not valid
                    if (Utilities.IsConditionNotValid(isValid, conditionOperation))
                        break;
                }
            }
            else
            {


                foreach (ConditionRange conditionRange in Utilities.GetRanges(condition.RightOperand))
                {
                    Enums.ConditionOperation conditionOperation = (Enums.ConditionOperation)condition.ConditionOperator;

                    //Check length is valid or not
                    isValid = Utilities.IsValidLength(conditionRange, condition.LeftOperands, conditionOperation);

                    if (!(!isValid && (conditionOperation == Enums.ConditionOperation.GreaterThan
                                       || conditionOperation == Enums.ConditionOperation.LessThan)))
                    {
                        //Pads left with spaces to make all items of same size
                        ConditionRange padConditionRange = Utilities.PadConditionRange(conditionRange);
                        Utilities.PadLeftToCode(condition.LeftOperands, padConditionRange);
                        isValid = IsAlphaNumericCodesExists(padConditionRange, condition,
                            conditionOperation, isHcpcsCode);
                    }

                    //Break loop is condition is not valid
                    if (Utilities.IsConditionNotValid(isValid, conditionOperation))
                        break;
                }
            }
            return isValid;
        }

        /// <summary>
        /// Determines whether [is alpha numeric codes exists] [the specified contract code].
        /// </summary>
        /// <param name="contractCode">The contract code.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="conditionOperation">The condition operation.</param>
        /// <param name="isHcpcsCode">if set to <c>true</c> [is HCPCS code].</param>
        /// <returns></returns>
        private bool IsAlphaNumericCodesExists(ConditionRange contractCode, ICondition condition,
            Enums.ConditionOperation conditionOperation, bool isHcpcsCode=false)
        {
            IEnumerable<string> claimCodes = condition.LeftOperands;
            //Default flag to false
            bool isValid = false;
            var enumerable = claimCodes == null ? new List<string>() : claimCodes.ToList();
            var emptyCodes = enumerable.ToList().Any(string.IsNullOrEmpty);

            if (emptyCodes && conditionOperation == Enums.ConditionOperation.NotEqualTo)
                isValid = true;
            else if (!(emptyCodes && conditionOperation == Enums.ConditionOperation.EqualTo))
            {
                foreach (string claimCode in enumerable.Select(code => code.ToLower()))
                {
                    isValid = IsClaimCodeExist(conditionOperation, claimCode, contractCode, isHcpcsCode);

                    //Break loop is condition is not valid
                    if (Utilities.IsConditionNotValid(isValid, conditionOperation))
                        break;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Determines whether [is claim code exist] [the specified condition operation].
        /// </summary>
        /// <param name="conditionOperation">The condition operation.</param>
        /// <param name="claimCode">The claim code.</param>
        /// <param name="conditionRange">The condition range.</param>
        /// <param name="isHcpcsCode">if set to <c>true</c> [is HCPCS code].</param>
        /// <returns></returns>
        private bool IsClaimCodeExist(Enums.ConditionOperation conditionOperation, string claimCode, ConditionRange conditionRange,bool isHcpcsCode=false)
        {
            bool isValid = false;
            switch (conditionOperation)
            {
                case Enums.ConditionOperation.EqualTo:
                    //If the claimcode is HCPCS, then claimcode will be considered as just first five characters of it.
                    isValid = conditionRange.EndValue != null
                        ? isHcpcsCode && claimCode.Trim()  != string.Empty? (String.CompareOrdinal(claimCode.Trim().Length >= 5 ? claimCode.Trim().Substring(0, 5) : claimCode,
                        conditionRange.StartValue.Trim().Length > 5 ? conditionRange.StartValue.Trim().Substring(0, 5) : conditionRange.StartValue.Trim()) >= 0 &&
                          String.CompareOrdinal(claimCode.Trim().Length >= 5 ? claimCode.Trim().Substring(0, 5) : claimCode,
                          conditionRange.EndValue.Trim().Length > 5 ? conditionRange.EndValue.Trim().Substring(0, 5) : conditionRange.EndValue.Trim()) <= 0) : (String.CompareOrdinal(claimCode,
                            conditionRange.StartValue) >= 0 &&
                          String.CompareOrdinal(claimCode,
                              conditionRange.EndValue) <= 0)
                        
                              : (conditionRange.StartValue.Contains(Constants.WildCard) && claimCode != null
                              ? new RegexHelper(conditionRange.StartValue.Trim().Length >= 5? conditionRange.StartValue.Trim().Substring(0,5).Replace(@"\", Constants.RegexHash):    conditionRange.StartValue.Trim().Replace(@"\", "#")).IsMatch(
                            isHcpcsCode && claimCode.Trim().Length > 5 ? claimCode.Trim().Substring(0, 5).Replace(@"\", Constants.RegexHash) : claimCode.Trim().Replace(@"\", Constants.RegexHash))
                            : String.CompareOrdinal(claimCode, conditionRange.StartValue) == 0);
                    if (!isValid && !string.IsNullOrWhiteSpace(claimCode) && isHcpcsCode && claimCode.Trim().Length > 5  && !conditionRange.StartValue.Contains(Constants.WildCard))
                    {
                        string claimCodeHcpcs = claimCode.Trim().Substring(0, 5);
                        isValid = conditionRange.EndValue != null
                            ? String.CompareOrdinal(claimCodeHcpcs,
                                conditionRange.StartValue) >= 0 &&
                              String.CompareOrdinal(claimCodeHcpcs,
                                  conditionRange.EndValue) <= 0
                            : (conditionRange.StartValue.Contains(Constants.WildCard)
                                ? new RegexHelper(conditionRange.StartValue.Trim().Replace(@"\", Constants.RegexHash)).IsMatch(
                                   claimCodeHcpcs.Replace(@"\", Constants.RegexHash))
                                : String.CompareOrdinal(claimCodeHcpcs,
                                    conditionRange.StartValue.Trim()) == 0);
                    }
                    break;
                case Enums.ConditionOperation.GreaterThan:
                    isValid = String.CompareOrdinal(claimCode,
                            conditionRange.StartValue) > 0;
                    break;
                case Enums.ConditionOperation.LessThan:
                    isValid = conditionRange.EndValue != null
                        ? String.CompareOrdinal(claimCode
                            , conditionRange.EndValue) < 0
                        : String.CompareOrdinal(claimCode,
                            conditionRange.StartValue) < 0;
                    break;
                case Enums.ConditionOperation.NotEqualTo:
                    claimCode = claimCode.Trim();
                    isValid = conditionRange.EndValue != null
                        ? !(String.CompareOrdinal(claimCode,
                            conditionRange.StartValue.Trim()) >= 0 &&
                            String.CompareOrdinal(claimCode,
                                conditionRange.EndValue.Trim()) <= 0)
                        : (conditionRange.StartValue.Contains(Constants.WildCard)
                            ? !new RegexHelper(conditionRange.StartValue.Trim().Replace(@"\", Constants.RegexHash)).IsMatch(
                                claimCode.Replace(@"\", Constants.RegexHash))
                            : String.CompareOrdinal(claimCode, conditionRange.StartValue.Trim()) != 0);
                    if (isValid && !string.IsNullOrWhiteSpace(claimCode) && isHcpcsCode && claimCode.Length > 5)
                    {
                        string claimCodeHcpcs = claimCode.Substring(0, 5);
                         isValid = conditionRange.EndValue != null
                        ? !(String.CompareOrdinal(claimCodeHcpcs,
                            conditionRange.StartValue) >= 0 &&
                            String.CompareOrdinal(claimCodeHcpcs,
                                conditionRange.EndValue) <= 0)
                        : (conditionRange.StartValue.Contains(Constants.WildCard)
                            ? !new RegexHelper(conditionRange.StartValue.Trim().Replace(@"\", Constants.RegexHash)).IsMatch(
                                claimCodeHcpcs.Replace(@"\", Constants.RegexHash))
                            : String.CompareOrdinal(claimCodeHcpcs, conditionRange.StartValue.Trim()) != 0);
                    }
                    break;
                case Enums.ConditionOperation.Contains:
                    isValid = claimCode.Trim().Contains(conditionRange.StartValue.Trim());
                    break;
            }
            return isValid;
        }

        /// <summary>
        /// Updates the code with include modifier.
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <returns></returns>
        private List<string> UpdateCodeWithIncludeModifier(IEnumerable<string> codes)
        {
            List<string> listOfCodes = new List<string>();
            foreach (List<string> codeList in codes.Select(code => code.Split(Constants.CommaCharacter).Select(item => item.Trim()).ToList()))
            {
                for (int codeIndex = 0; codeIndex < codeList.Count; codeIndex++)
                {
                    List<string> rangeCodeList = codeList[codeIndex].Split(Constants.RangeOperator).ToList();

                    int[] indexes = Enumerable.Range(0, rangeCodeList.Count).Where
                        (i => rangeCodeList[i] != null).ToArray();
                    Array.ForEach(indexes, i => rangeCodeList[i] = rangeCodeList[i] = rangeCodeList[i]);

                    codeList[codeIndex] = string.Join(Constants.RangeOperator.ToString(CultureInfo.InvariantCulture), rangeCodeList);
                }
                listOfCodes.Add(string.Join(Constants.Comma, codeList));
            }
            return listOfCodes;
        }

    }
}