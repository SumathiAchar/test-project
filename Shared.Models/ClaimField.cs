/************************************************************************************************************/
/**  Author         : Vishesh Bhawsar
/**  Created        : 23-Aug-2013
/**  Summary        : Handles Claim Field
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    public class ClaimField : BaseModel
    {
        /// <summary>
        /// Gets or sets the claim field id.
        /// </summary>
        /// <value>
        /// The claim field id.
        /// </value>
        public long ClaimFieldId { get; set; }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
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
        /// Gets or sets the TableName.
        /// </summary>
        /// <value>
        /// TableName.
        /// </value>
        public string TableName { get; set; }
    }
}
