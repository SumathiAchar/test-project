using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{

    public class PaymentTypeCustomTable : PaymentTypeBase
    {
        /// <summary>
        /// Gets or sets the Claim FieldDoc ID.
        /// </summary>
        /// <value>The uploaded document ID.</value>
        public long? DocumentId { get; set; }
        /// <summary>
        /// Gets or sets the Claim Field Id.
        /// </summary>
        /// <value>The Claim Field Id.</value>
        public long? ClaimFieldId { get; set; }

        // ReSharper disable once UnusedMember.Global
        // This property is used in Report
        public long? NodeId { get; set; }
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
        /// Gets or sets the claim field document identifier.
        /// </summary>
        /// <value>
        /// The claim field document identifier.
        /// </value>
        public long? ClaimFieldDocId { get; set; }

        /// <summary>
        /// Gets or sets the claim field document.
        /// </summary>
        /// <value>
        /// The claim field document.
        /// </value>
        public ClaimFieldDoc ClaimFieldDoc { get; set; }

        /// <summary>
        /// Gets or sets the multiplier first.
        /// </summary>
        public string MultiplierFirst { get; set; }

        /// <summary>
        /// Gets or sets the multiplier second.
        /// </summary>
        public string MultiplierSecond { get; set; }

        /// <summary>
        /// Gets or sets the multiplier third.
        /// </summary>
        public string MultiplierThird { get; set; }

        /// <summary>
        /// Gets or sets the multiplier fourth.
        /// </summary>
        public string MultiplierFourth { get; set; }

        /// <summary>
        /// Gets or sets the multiplier other.
        /// </summary>
        public string MultiplierOther { get; set; }

        /// <summary>
        /// Gets or sets the observe unit check box.
        /// </summary>
        public bool IsObserveServiceUnit { get; set; }

        /// <summary>
        /// Gets or sets the observe service unit limit.
        /// </summary>
        public string ObserveServiceUnitLimit { get; set; }

        /// <summary>
        /// Gets or sets the per day of stay.
        /// </summary>
        public bool IsPerDayOfStay { get; set; }

        /// <summary>
        /// Gets or sets the per code.
        /// </summary>
        public bool IsPerCode { get; set; }

        /// <summary>
        /// Gets or sets the total limit.
        /// </summary>
        /// <value>
        /// The total limit.
        /// </value>
        public int TotalLimit { get; set; }

    }
}
