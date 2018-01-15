using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// For a condition like Revcode = 102,103 in Claimdata which has revcodes=101,102,103,104,105
    /// ConditionOperator = Enumerators.ConditionOperation.EqualTo
    /// OperandType = Enumerators.OperandType.Numeric
    /// OperandIdentifier = Enumerators.OperandIdentifier.RevenueCode
    /// LeftOperand = new List of string (){101,102,103,104,105} (From Claim)
    /// RightOperand = "102,103"(From Contract Condition)
    /// </summary>
    public class Condition : ICondition
    {
        /// <summary>
        /// Gets or sets the condition operator.
        /// </summary>
        /// <value>
        /// The condition operator.
        /// </value>
        public int ConditionOperator { get; set; }


        /// <summary>
        /// Gets or sets the type of the operand.
        /// </summary>
        /// <value>
        /// The type of the operand.
        /// </value>
        public int OperandType { get; set; }

        /// <summary>
        /// Gets or sets the left operand.
        /// </summary>
        /// <value>
        /// The left operand.
        /// </value>
        public int? OperandIdentifier { get; set; }


        /// <summary>
        /// Gets or sets the right operand.
        /// </summary>
        /// <value>
        /// The right operand.
        /// </value>
        public string RightOperand { get; set; }

        /// <summary>
        /// Gets or sets the left operand values. For eg. Revcode= 101,102,103
        /// </summary>
        /// <value>
        /// The left operand.
        /// </value>
        public List<string> LeftOperands { get; set; }

        /// <summary>
        /// Gets or sets the Property Column Name.
        /// </summary>
        /// <value>
        /// Property Column Name.
        /// </value>
        public string PropertyColumnName { get; set; }

       public Condition Clone()
        {
            return (Condition)MemberwiseClone();
        }
        
    }
}
