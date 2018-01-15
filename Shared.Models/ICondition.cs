using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// A generalized condition representation capable of determining if it is a match against any object with the IEvaluateableClaim interface.
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// Gets or sets the condition operator.
        /// </summary>
        /// <value>
        /// The condition operator.
        /// </value>
        int ConditionOperator { get; set; }

        /// <summary>
        /// Gets or sets the type of the operand.
        /// </summary>
        /// <value>
        /// The type of the operand.
        /// </value>
        int OperandType { get; set; }

        /// <summary>
        /// Gets or sets the left operand.
        /// </summary>
        /// <value>
        /// The left operand.
        /// </value>
        int? OperandIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the right operand.
        /// </summary>
        /// <value>
        /// The right operand.
        /// </value>
        string RightOperand { get; set; }

        /// <summary>
        /// Gets or sets the left operand values. For eg. Revcode= 101,102,103
        /// </summary>
        /// <value>
        /// The left operand.
        /// </value>
        List<string> LeftOperands { get; set; }

        /// <summary>
        /// Gets or sets the Property Column Name.
        /// </summary>
        /// <value>
        /// Property Column Name.
        /// </value>
        string PropertyColumnName { get; set; }

        Condition Clone();
        
    }
}
