/************************************************************************************************************/
/**  Author         : R Bhandari
/**  Created        : 25/Aug/2013
/**  Summary        : Handles Contract Modified Reason
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // No need to Instantiated
    public class ContractModifiedReason : BaseModel
    {
        /// <summary>
        /// Set or Get Contract Id.
        /// </summary>
        /// <value>
        /// The Contract Id.
        /// </value>
        public long? ContractId { set; get; }


        /// <summary>
        /// Set or Get Reason Code.
        /// </summary>
        /// <value>
        /// The Reason Code.
        /// </value>
        public long? ReasonCode { set; get; }



        /// <summary>
        /// Set or Get Notes.
        /// </summary>
        /// <value>
        /// The Notes.
        /// </value>
        public string Notes { set; get; }

        /// <summary>
        /// Set or Get Contract Id.
        /// </summary>
        /// <value>
        /// The Contract Id.
        /// </value>
        public long? NodeId { set; get; }
    }
}
