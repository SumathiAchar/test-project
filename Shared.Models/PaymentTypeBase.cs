using System.Collections.Generic;
using System.Xml.Serialization;

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Base Model for payment type
    /// </summary>
    public abstract class PaymentTypeBase : BaseModel, IPaymentType
    {

        /// <summary>
        /// Gets or sets the payment type detail identifier.
        /// </summary>
        /// <value>
        /// The payment type detail identifier.
        /// </value>
        public long PaymentTypeDetailId { set; get; }

        /// <summary>
        /// Gets or sets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public long? ContractId { set; get; }

        /// <summary>
        /// Gets or sets the payment type identifier.
        /// </summary>
        /// <value>
        /// The payment type identifier.
        /// </value>
        public int PaymentTypeId { set; get; }

        /// <summary>
        /// Gets or sets the service type identifier.
        /// </summary>
        /// <value>
        /// The service type identifier.
        /// </value>
        public long? ServiceTypeId { set; get; }

        /// <summary>
        /// Gets or sets the rev code.
        /// </summary>
        /// <value>
        /// The rev code.
        /// </value>
        public string RevCode { get; set; }

        /// <summary>
        /// Gets or sets the HCPCS code.
        /// </summary>
        /// <value>
        /// The HCPCS code.
        /// </value>
        public string HcpcsCode { get; set; }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>
        /// The conditions.
        /// </value>
          [XmlIgnore]
        public abstract List<ICondition> Conditions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [pay at claim level].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [pay at claim level]; otherwise, <c>false</c>.
        /// </value>
        public abstract bool PayAtClaimLevel { get; set; }

        /// <summary>
        /// Gets or sets the valid line ids.
        /// </summary>
        /// <value>
        /// The valid line ids.
        /// </value>
        public abstract List<int> ValidLineIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is formula error.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is formula error; otherwise, <c>false</c>.
        /// </value>
        public bool IsFormulaError { get; set; }

        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>
        /// The expression.
        /// </value>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the expanded expression.
        /// </summary>
        /// <value>
        /// The expanded expression.
        /// </value>
        public string ExpandedExpression { get; set; }

        /// <summary>
        /// Gets or sets the first expanded expression.
        /// </summary>
        /// <value>
        /// The first expanded expression.
        /// </value>
        public bool IsMultiplierFormulaError { get; set; }

        /// <summary>
        /// Gets or sets the multiplier expanded expression.
        /// </summary>
        /// <value>
        /// The multiplier expanded expression.
        /// </value>
        public string MultiplierExpandedExpression { get; set; }

        /// <summary>
        /// Gets or sets the multiplier expression result.
        /// </summary>
        /// <value>
        /// The multiplier expression result.
        /// </value>
        public double? MultiplierExpressionResult { get; set; }

        /// <summary>
        /// Gets or sets the multiplier.
        /// </summary>
        /// <value>
        /// The multiplier.
        /// </value>
        public int Multiplier { get; set; }

        /// <summary>
        /// Gets or sets the limit expanded expression.
        /// </summary>
        /// <value>s
        /// The limit expanded expression.
        /// </value>
        public string LimitExpandedExpression { get; set; }

        /// <summary>
        /// Gets or sets the limit expression result.
        /// </summary>
        /// <value>
        /// The limit expression result.
        /// </value>
        public double? LimitExpressionResult { get; set; }

        /// <summary>
        /// Gets or sets the limit expression result.
        /// </summary>
        /// <value>
        /// The limit expression result.
        /// </value>
        public double? FormulaExpressionResult { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is others multiplier error.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is others multiplier error; otherwise, <c>false</c>.
        /// </value>
        public bool IsLimitError { get; set; }

    }
}
