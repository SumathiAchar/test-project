/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 24-Dec-2014
/**  Summary        : Handles Payment Type ASC Fee Schedule
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class PaymentTypeAscFeeSchedule : PaymentTypeBase
    {
        /// <summary>
        /// Gets or sets the primary.
        /// </summary>
        /// <value>The primary.</value>
        public double? Primary { get; set; }
        /// <summary>
        /// Gets or sets the secondary.
        /// </summary>
        /// <value>The secondary.</value>
        public double? Secondary { get; set; }
        /// <summary>
        /// Gets or sets the tertiary.
        /// </summary>
        /// <value>The tertiary.</value>
        public double? Tertiary { get; set; }
        /// <summary>
        /// Gets or sets the quaternary.
        /// </summary>
        /// <value>The quaternary.</value>
        public double? Quaternary { get; set; }
        /// <summary>
        /// Gets or sets the others.
        /// </summary>
        /// <value>The others.</value>
        public double? Others { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        public override List<ICondition> Conditions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override bool PayAtClaimLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public override List<int> ValidLineIds { get; set; }

        /// <summary>
        /// Gets or sets the Claim FieldDoc ID.
        /// </summary>
        /// <value>The Claim FieldDoc ID.</value>
        public long? ClaimFieldDocId { get; set; }
        /// <summary>
        /// Gets or sets the Claim Field Id.
        /// </summary>
        /// <value>The Claim Field Id.</value>
        public long? ClaimFieldId { get; set; }

        /// <summary>
        /// Gets or sets the non fee schedule.
        /// </summary>
        /// <value>The non fee schedule.</value>
        public double? NonFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the claim field document.
        /// </summary>
        /// <value>
        /// The claim field document.
        /// </value>
        public ClaimFieldDoc ClaimFieldDoc { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        public long? NodeId { get; set; }

        /// <summary>
        /// Gets or sets the option selection.
        /// </summary>
        /// <value>
        /// The option selection.
        /// </value>
        public int OptionSelection { get; set; }

        /// <summary>
        /// Gets or sets the user value.
        /// </summary>
        /// <value>
        /// The user value.
        /// </value>
        public string UserText { get; set; }
    }
}
