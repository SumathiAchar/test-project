/************************************************************************************************************/
/**  Author         : G Mohanty
/**  Created        : 21-Aug-2013
/**  Summary        : Handles Payment Type Medicare OP Payment
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class PaymentTypeMedicareOp: PaymentTypeBase
    {
        /// <summary>
        /// Gets or sets the Out Patient.
        /// </summary>
        /// <value>The Out Patient.</value>
        public double? OutPatient { set; get; }

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
        /// Gets or sets a value indicating whether [medicare op apc].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [medicare op apc]; otherwise, <c>false</c>.
        /// </value>
        public bool IsMedicareOpApcEnabled { get; set; }
    }
}
