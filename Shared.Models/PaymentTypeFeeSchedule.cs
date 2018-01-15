/************************************************************************************************************/
/**  Author         : G Mohanty
/**  Created        : 21-Aug-2013
/**  Summary        : Handles Payment Type Fee Schedules
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Fee Schedule Payment Type
    /// </summary>
    public class PaymentTypeFeeSchedule : PaymentTypeBase
    {
        /// <summary>
        /// Gets or sets the fee schedule.
        /// </summary>
        /// <value>
        /// The fee schedule.
        /// </value>
        public double? FeeSchedule { set; get; }

        /// <summary>
        /// Gets or sets the non fee schedule.
        /// </summary>
        /// <value>
        /// The non fee schedule.
        /// </value>
        public double? NonFeeSchedule { set; get; }

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
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>The conditions.</value>
        public override List<ICondition> Conditions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [pay at claim level].
        /// </summary>
        /// <value><c>true</c> if [pay at claim level]; otherwise, <c>false</c>.</value>
        public override bool PayAtClaimLevel { get; set; }

        /// <summary>
        /// Gets or sets the valid line ids.
        /// </summary>
        /// <value>The valid line ids.</value>
        public override List<int> ValidLineIds { get; set; }

        /// <summary>
        /// Gets or sets the IsObserveUnits 
        /// </summary>
        public bool IsObserveUnits { get; set; }

    }
}
