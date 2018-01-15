/************************************************************************************************************/
/**  Author         :V Bhaswar
/**  Created        :19/Aug/2013
/**  Summary        : Handles Contract Service Line Table Selection
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractServiceLineTableSelection:BaseModel
    {
        /// <summary>
        /// Gets or sets the contract service line table selection id.
        /// </summary>
        /// <value>
        /// The contract service line table selection id.
        /// </value>
        public long ContractServiceLineTableSelectionId { get; set; }
        
        /// <summary>
        /// Gets or sets the contract service type id.
        /// </summary>
        /// <value>
        /// The contract service type id.
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
        /// Gets or sets the name of the contract service line table.
        /// </summary>
        /// <value>
        /// The name of the contract service line table.
        /// </value>
        public string TableName { get; set; }
        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        public string Field { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int? Status { get; set; }

        /// <summary>
        /// Gets or sets the ClaimFieldId.
        /// </summary>
        /// <value>
        /// The ClaimFieldId.
        /// </value>
        public long ClaimFieldId { get; set; }

        /// <summary>
        /// Gets or sets the ClaimFieldText.
        /// </summary>
        /// <value>
        /// The Text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the ClaimFieldDocId.
        /// </summary>
        /// <value>
        /// The ClaimFieldDocId.
        /// </value>
        public long ClaimFieldDocId { get; set; }

        /// <summary>
        /// Gets or sets the ServiceLineTypeId.
        /// </summary>
        /// <value>
        /// The ServiceLineTypeId.
        /// </value>
        public long? ServiceLineTypeId { get; set; }

        /// <summary>
        /// Gets or sets the contract service line identifier.
        /// </summary>
        /// <value>
        /// The contract service line identifier.
        /// </value>
        public long ContractServiceLineId { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public int Operator { get; set; }

        /// <summary>
        /// Gets or sets the type of the operator.
        /// </summary>
        /// <value>
        /// The type of the operator.
        /// </value>
        public string OperatorType { get; set; }

        /// <summary>
        /// Gets or sets the type of the table.
        /// </summary>
        /// <value>
        /// The type of the table.
        /// </value>
        public int? TableType { get; set; }

        /// <summary>
        /// Gets or sets the user text.
        /// </summary>
        /// <value>
        /// The user text.
        /// </value>
        public string UserText { get; set; }
    }
}
