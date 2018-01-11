using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    /// <summary>
    /// Base class for the contract related logic
    /// </summary>
    public class ContractBaseLogic
    {
        /// <summary>
        /// The _condition logic
        /// </summary>
        private readonly IConditionLogic _conditionLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractBaseLogic"/> class.
        /// </summary>
        public ContractBaseLogic()
        {
            _conditionLogic = Factory.CreateInstance<IConditionLogic>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractBaseLogic"/> class.
        /// </summary>
        /// <param name="conditionLogic">The condition logic.</param>
        public ContractBaseLogic(IConditionLogic conditionLogic)
        {
            if (conditionLogic != null)
                _conditionLogic = conditionLogic;
        }

        /// <summary>
        /// Determines whether [is conditions valid] [the specified conditions].
        /// </summary>
        /// <param name="conditions">The conditions.</param>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public bool IsConditionsValid(List<ICondition> conditions, IEvaluateableClaim claim)
        {
            if (conditions != null && conditions.Any())
                //Fill LHS oprand of each contract with claim codes
                lock (conditions)
                {
                    // ReSharper disable once ForCanBeConvertedToForeach
                    // for loop is required for multiple threads
                    for (int index = 0; index < conditions.Count; index++)
                    {
                        ICondition condition = conditions[index];
                        _conditionLogic.Condition = condition;
                        condition.LeftOperands =
                            _conditionLogic.GetPropertyValues(claim, _conditionLogic.Condition.PropertyColumnName);

                    }
                }

            bool isMatch = true;

            //Validate each condition in a contract
            if (conditions != null)
            {
                lock (conditions)
                {
                    // ReSharper disable once ForCanBeConvertedToForeach
                    // for loop is required for multiple threads
                    for (int index = 0; index < conditions.Count; index++)
                    {
                        ICondition condition = conditions[index];
                        _conditionLogic.Condition = condition;
                        isMatch = _conditionLogic.IsMatch();
                        if (!isMatch)
                            break;
                    }
                }
            }
            return isMatch;
        }
    }
}

