using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Helpers.ConditionValidator;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// Class for Contract Logic
    /// </summary>
    public class ConditionLogic : IConditionLogic
    {
        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public ICondition Condition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is match].
        /// </summary>
        /// <returns></returns>
        /// <value>
        ///   <c>true</c> if [is match]; otherwise, <c>false</c>.
        /// </value>
        public bool IsMatch()
        {
            //Based on operand Type inject respective validation logic.
            IConditionValidator conditionValidator = Factory.CreateInstance<IConditionValidator>(((Enums.OperandType)Condition.OperandType).ToString());
            return conditionValidator.IsValid(Condition);
        }

        /// <summary>
        /// Gets the property values.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<string> GetPropertyValues(object claim, string propertyName)
        {
            List<string> values = new List<string>();

            if (claim != null && !GetPhysicianData(claim, propertyName, ref values) &&
               !GetStatementCoverPeriod(claim, propertyName, ref values))
            {
                GetPropertyValues(claim, propertyName, ref values);
            }
            return values;
        }

        /// <summary>
        /// Gets the property values.
        /// </summary>
        /// <param name="currentObject">The current object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="values">The values.</param>
        private void GetPropertyValues(object currentObject, string propertyName, ref List<string> values)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                //propertyName names like Claim.ClaimCharges, Claim.ClaimCharges.Revcode
                if (propertyName.Contains('.'))
                {
                    List<string> fieldNames = propertyName.Split('.').ToList();
                    var childData = GetValue(currentObject, fieldNames[0]);

                    if (childData != null)
                        foreach (var item in (IEnumerable<object>) childData)
                        {
                            string newPropertyName = String.Join(".",
                                fieldNames.Where(fieldName => fieldName != fieldNames[0]));
                            values.AddRange(GetPropertyValues(item, newPropertyName));
                        }
                }
                else
                {
                    values.Add(Convert.ToString(GetValue(currentObject, propertyName), CultureInfo.InvariantCulture));    
                }
            }
        }

        /// <summary>
        /// Gets the statement cover period.
        /// </summary>
        /// <param name="currentObject">The current object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private static bool GetStatementCoverPeriod(object currentObject, string propertyName, ref List<string> values)
        {
            if (!string.IsNullOrEmpty(propertyName) && propertyName.Equals(Constants.PropertyStatementCoversPeriod))
            {
                EvaluateableClaim evaluateableClaim = currentObject as EvaluateableClaim;
                if (evaluateableClaim != null)
                {
                    values.Add(evaluateableClaim.StatementFrom.ToString());
                    values.Add(evaluateableClaim.StatementThru.ToString());
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the physician data.
        /// </summary>
        /// <param name="currentObject">The current object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private static bool GetPhysicianData(object currentObject, string propertyName, ref List<string> values)
        {
            if (!string.IsNullOrEmpty(propertyName) && (propertyName.Equals(Constants.PropertyReferringPhysician) ||
                propertyName.Equals(Constants.PropertyRenderingPhysician) ||
                propertyName.Equals(Constants.PropertyAttendingPhysician)))
            {
                EvaluateableClaim evaluateableClaim = currentObject as EvaluateableClaim;
                if (evaluateableClaim != null && evaluateableClaim.Physicians != null &&
                                                  evaluateableClaim.Physicians.Any(a => a.PhysicianType == propertyName))
                {
                    values.AddRange(
                        evaluateableClaim.Physicians.Where(a => a.PhysicianType == propertyName).
                            Select(
                                a =>
                                    string.IsNullOrEmpty(a.MiddleName)
                                        ? (a.FirstName + " " + a.LastName).Trim().Replace(" ", "")
                                        : (a.FirstName + " " + a.MiddleName + " " + a.LastName).Trim
                                            ().Replace(" ", "")).ToList());
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the value of an object based on propertyName.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private static object GetValue(object component, string propertyName)
        {
            return TypeDescriptor.GetProperties(component)[propertyName].GetValue(component);
        }

    }
}
