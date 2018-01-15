/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 29-Dec-2014
/**  Summary        : Handles Payment Type DRG Schedules
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class PaymentTypeDrg : PaymentTypeBase
    {

        /// <summary>
        /// Gets or sets the base rate.
        /// </summary>
        /// <value>The base rate.</value>
        public double? BaseRate { get; set; }
        /// <summary>
        /// Set or Get RelativeWeightId of PaymentTypeDRGPayment Table
        /// </summary>
        public long? RelativeWeightId { set; get; }

        /// <summary>
        /// holds list of conditions at Payment type level
        /// </summary>
        public override List<ICondition> Conditions { get; set; }

        /// <summary>
        /// returns true if payment has to be made at claim level
        /// </summary>
        public override bool PayAtClaimLevel { get; set; }

        /// <summary>
        /// Holds the line ids which matches the given criteria
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
        // ReSharper disable once UnusedMember.Global
        public long? ClaimFieldId { get; set; }

        /// <summary>
        /// Gets or sets the claim field document.
        /// </summary>
        /// <value>
        /// The claim field document.
        /// </value>
        public ClaimFieldDoc ClaimFieldDoc { get; set; }
    }
}
