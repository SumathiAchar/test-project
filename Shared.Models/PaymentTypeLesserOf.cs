using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class PaymentTypeLesserOf : PaymentTypeBase
    {
        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>The percentage.</value>
        public double? Percentage { get; set; }
        
        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>
        /// The conditions.
        /// </value>
        public override List<ICondition> Conditions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [pay at claim level].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [pay at claim level]; otherwise, <c>false</c>.
        /// </value>
        public override bool PayAtClaimLevel { get; set; }

        /// <summary>
        /// Gets or sets the valid line ids.
        /// </summary>
        /// <value>
        /// The valid line ids.
        /// </value>
        public override List<int> ValidLineIds { get; set; }

        /// <summary>
        /// Gets or sets the is lesser of.
        /// </summary>
        /// <value>
        /// The is lesser of.
        /// </value>
        public bool? IsLesserOf { get; set; }
    }
}
