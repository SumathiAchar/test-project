using System;
using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Helpers.ConditionValidator
{
    /// <summary>
    /// Class ValidateNumericCodes.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class NumericConditionValidator : IConditionValidator
    {
        /// <summary>
        /// Determines whether the specified condition is valid.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        public bool IsValid(ICondition condition)
        {
            return IsNumericCodeExist(condition);
        }

        /// <summary>
        /// Determines whether [is numeric code exist] [the specified condition].
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        private bool IsNumericCodeExist(ICondition condition)
        {
            bool isValid = false;

            var enumerable = condition.LeftOperands as IList<string> ?? condition.LeftOperands.ToList();
            var emptyCodes = enumerable.ToList().Any(string.IsNullOrEmpty);

            Enums.ConditionOperation conditionOperation = (Enums.ConditionOperation)condition.ConditionOperator;

            if (emptyCodes && conditionOperation == Enums.ConditionOperation.NotEqualTo)
                isValid = true;
            else if (!(emptyCodes && conditionOperation == Enums.ConditionOperation.EqualTo))
            {
                //Getting Start and End values (Rage) from string
                List<ConditionRange> conditionRanges = Utilities.GetRanges(condition.RightOperand);

                //Update Total Charge Data
                conditionRanges = UpdateTotalChargeData(conditionRanges, condition);

                //For each claim codes match with contract conditions.
                foreach (string claimCode in condition.LeftOperands)
                {
                    foreach (ConditionRange conditionRange in conditionRanges)
                    {
                        //check claim codes is between Start and End value range.
                        isValid = IsCodeExist(conditionOperation, claimCode, conditionRange);

                        //Break loop is condition is not valid
                        if (Utilities.IsConditionNotValid(isValid, conditionOperation))
                            break;
                    }

                    //Break loop is condition is not valid
                    if (Utilities.IsConditionNotValid(isValid, conditionOperation))
                        break;

                }
            }
            return isValid;
        }

        /// <summary>
        /// Determines whether [is code exist] [the specified condition operation].
        /// </summary>
        /// <param name="conditionOperation">The condition operation.</param>
        /// <param name="claimCode">The claim code.</param>
        /// <param name="conditionRange">The condition range.</param>
        /// <returns></returns>
        private bool IsCodeExist(Enums.ConditionOperation conditionOperation, string claimCode, ConditionRange conditionRange)
        {
            bool isValid = false;
            double numericCode;
            double numericStartValue;
            if (Double.TryParse(claimCode, out numericCode) && Double.TryParse(conditionRange.StartValue, out numericStartValue))
            {
                double numericEndValue;
                switch (conditionOperation)
                {
                    case Enums.ConditionOperation.EqualTo:
                        isValid = conditionRange.EndValue != null
                            ? Double.TryParse(conditionRange.EndValue, out numericEndValue) &&
                              (numericCode >= numericStartValue) &&
                              (numericCode <= numericEndValue)
                            : numericCode.Equals(numericStartValue);
                        break;
                    case Enums.ConditionOperation.GreaterThan:
                        isValid = numericCode > numericStartValue;
                        break;
                    case Enums.ConditionOperation.LessThan:
                        isValid = numericCode < numericStartValue;
                        break;
                    case Enums.ConditionOperation.NotEqualTo:
                        isValid = conditionRange.EndValue != null
                            ? Double.TryParse(conditionRange.EndValue, out numericEndValue) &&
                              !((numericCode >= numericStartValue) &&
                                (numericCode <= numericEndValue))
                            : !numericCode.Equals(numericStartValue);
                        break;
                }
            }
            else if (conditionRange.StartValue.Contains('*') && claimCode != null && conditionRange.EndValue == null)
            {
                switch (conditionOperation)
                {
                    case Enums.ConditionOperation.EqualTo:
                        isValid =
                            new RegexHelper(conditionRange.StartValue.Trim().Replace(@"\", "#")).IsMatch(claimCode.Trim().Replace(
                                @"\", "#"));
                        break;
                    case Enums.ConditionOperation.NotEqualTo:
                        isValid =
                            !new RegexHelper(conditionRange.StartValue.Trim().Replace(@"\", "#")).IsMatch(
                                claimCode.Trim().Replace(@"\", "#"));
                        break;

                }
            }
            return isValid;
        }


        /// <summary>
        /// Updates the total charge data.
        /// </summary>
        /// <param name="conditionRanges">The condition ranges.</param>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        private List<ConditionRange> UpdateTotalChargeData(List<ConditionRange> conditionRanges, ICondition condition)
        {
            if (conditionRanges != null)
            {
                foreach (ConditionRange conditionRange in conditionRanges.Where(conditionRange => condition.OperandIdentifier == (byte)Enums.OperandIdentifier.TotalCharges))
                {
                    conditionRange.StartValue = conditionRange.StartValue.Replace("$", string.Empty);

                    if (conditionRange.EndValue != null)
                        conditionRange.EndValue = conditionRange.EndValue.Replace("$", string.Empty);
                }
            }
            return conditionRanges;
        }

    }
}
