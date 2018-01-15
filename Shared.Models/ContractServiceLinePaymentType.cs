/************************************************************************************************************/
/**  Author         :V Baaswar
/**  Created        :19/Aug/2013
/**  Summary        : Handles Contract Service Line Payment Types
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractServiceLinePaymentType:BaseModel
    {
        /// <summary>
        /// Gets or sets the payment type detail id.
        /// </summary>
        /// <value>
        /// The payment type detail id.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public long PaymentTypeDetailId { get; set; }
        
        /// <summary>
        /// Gets or sets the payment type id.
        /// </summary>
        /// <value>
        /// The payment type id.
        /// </value>
        public long? PaymentTypeId { get; set; }
        /// <summary>
        /// Gets or sets the contract service line id.
        /// </summary>
        /// <value>
        /// The contract service line id.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public long? ContractServiceLineId { get; set; }
        /// <summary>
        /// Gets or sets the contract id.
        /// </summary>
        /// <value>
        /// The contract id.
        /// </value>
        public long? ContractId { get; set; }
        /// <summary>
        /// Gets or sets the contract id.
        /// </summary>
        /// <value>
        /// The contract id.
        /// </value>
        public long? ServiceLineTypeId { get; set; }
            /// <summary>
        /// Gets or sets the contract id.
        /// </summary>
        /// <value>
        /// The contract id.
        /// </value>
        public long? ContractServiceTypeId { get; set; }

    }
}
