/************************************************************************************************************/
/**  Author         : G Mohanty
/**  Created        : 21-Aug-2013
/**  Summary        : Handles Payment Type Medicare IP Payment
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class PaymentTypeMedicareIp: PaymentTypeBase
    {
        /// <summary>
        /// Gets or sets the information patient.
        /// </summary>
        /// <value>The information patient.</value>
        public double? InPatient { set; get; }

        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        /// <value>
        /// The formula.
        /// </value>
        public string Formula { set; get; }

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
        /// Gets or sets the medicare ip acute options.
        /// </summary>
        /// <value>
        /// The medicare ip acute options.
        /// </value>
        public List<MedicareIpAcuteOption> MedicareIpAcuteOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [medicare ip acute].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [medicare ip acute]; otherwise, <c>false</c>.
        /// </value>
        public bool IsMedicareIpAcuteEnabled { get; set; }
    }
}
