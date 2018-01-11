using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Helpers.ConditionValidator
{
    /// <summary>
    /// Class to Validate date values.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class DateConditionValidator : IConditionValidator
    {
        /// <summary>
        /// Determines whether the specified condition is valid.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns><c>true</c> if the specified condition is valid; otherwise, <c>false</c>.</returns>
        public bool IsValid(ICondition condition)
        {
            return IsDateExist(condition);
        }


        /// <summary>
        /// Determines whether [is date exist] [the specified condition].
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns></returns>
        private bool IsDateExist(ICondition condition)
        {
            bool isValid = false;

            //Both Date Should come in LeftOperands for StatementCoversPeriodToDatesOfService. that's why we are checking Count = 2. 
            bool isStatementCoversPeriod = condition.OperandIdentifier == (byte)
                                           Enums.OperandIdentifier.StatementCoversPeriodToDatesOfService &&
                                           condition.LeftOperands.Count == 2;

            foreach (ConditionRange conditionRange in Utilities.GetRanges(condition.RightOperand))
            {
                Enums.ConditionOperation conditionOperation = (Enums.ConditionOperation)condition.ConditionOperator;

                DateTime date;
                if (DateTime.TryParse(conditionRange.StartValue, out date))
                {
                    switch (conditionOperation)
                    {
                        case Enums.ConditionOperation.EqualTo:
                            isValid = isStatementCoversPeriod
                                ? condition.LeftOperands[0] != null && condition.LeftOperands[1] != null &&
                                  (date.Date >= Convert.ToDateTime(condition.LeftOperands[0]) &&
                                   date.Date <= Convert.ToDateTime(condition.LeftOperands[1]))
                                : condition.LeftOperands[0] != null && date.Date == Convert.ToDateTime(condition.LeftOperands[0]);
                            break;
                        case Enums.ConditionOperation.GreaterThan:
                            isValid = isStatementCoversPeriod
                                ? condition.LeftOperands[1] != null &&
                                      (Convert.ToDateTime(condition.LeftOperands[1]) > date.Date)
                                      : condition.LeftOperands[0] != null && Convert.ToDateTime(condition.LeftOperands[0]) > date.Date;
                            break;
                        case Enums.ConditionOperation.LessThan:
                            isValid = condition.LeftOperands[0] != null &&
                                      Convert.ToDateTime(condition.LeftOperands[0]) < date.Date;
                            break;
                        case Enums.ConditionOperation.NotEqualTo:
                            isValid = isStatementCoversPeriod
                                ? condition.LeftOperands[0] != null && condition.LeftOperands[1] != null &&
                                      (!(date.Date >= Convert.ToDateTime(condition.LeftOperands[0]) &&
                                         date.Date <= Convert.ToDateTime(condition.LeftOperands[1])))
                                         : condition.LeftOperands[0] != null && date.Date != Convert.ToDateTime(condition.LeftOperands[0]);
                            break;
                        case Enums.ConditionOperation.GreaterThanEqualTo:
                            isValid = condition.LeftOperands[0] != null &&
                                      Convert.ToDateTime(condition.LeftOperands[0]) >= date.Date;
                            break;
                        case Enums.ConditionOperation.LessThanEqualTo:
                            isValid = condition.LeftOperands[0] != null &&
                                      Convert.ToDateTime(condition.LeftOperands[0]) <= date.Date;
                            break;
                    }
                }
                //Break loop is condition is not valid
                if (Utilities.IsConditionNotValid(isValid, conditionOperation))
                    break;
            }
            return isValid;
        }
    }
}
