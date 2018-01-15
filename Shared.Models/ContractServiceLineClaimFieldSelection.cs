/************************************************************************************************************/
/**  Author         :v Bhaswar
/**  Created        :19/Aug/2013
/**  Summary        : Handles Contract ServiceLine Claim Field Selection
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractServiceLineClaimFieldSelection : BaseModel
    {
        /// <summary>
        /// Gets or sets the contract service line claim field id.
        /// </summary>
        /// <value>
        /// The contract service line claim field id.
        /// </value>
        public long ContractServiceLineClaimFieldId { get; set; }

        /// <summary>
        /// Gets or sets the claim field id.
        /// </summary>
        /// <value>
        /// The claim field id.
        /// </value>
        public long? ClaimFieldId { get; set; }

        /// <summary>
        /// Gets or sets the contract service type ID.
        /// </summary>
        /// <value>
        /// The contract service type ID.
        /// </value>
        public long? ContractServiceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the contract id.
        /// </summary>
        /// <value>
        /// The contract id.
        /// </value>
        public long? ContractId { get; set; }
        /// <summary>
        /// Gets or sets the claim field.
        /// </summary>
        /// <value>
        /// The claim field.
        /// </value>
        public string ClaimField { get; set; }

        /// <summary>
        /// Gets or sets the OperatorType.
        /// </summary>
        /// <value>
        /// The OperatorType.
        /// </value>
        public string OperatorType { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public int Operator { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public string Values { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        //public List<string> ClaimFieldValues { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int? Status { get; set; }
        /// <summary>
        /// Gets or sets the ServiceLineType Id.
        /// </summary>
        /// <value>
        /// The ServiceLineType Id.
        /// </value>
        public long? ServiceLineTypeId { get; set; }

        // ReSharper disable once UnusedMember.Global
        public int ModuleId { get; set; }
    }
}
