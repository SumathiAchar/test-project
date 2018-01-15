/*******************************************************************************************************/
/**  Author         : Prasad 
/**  Created        : 30-Sep-2013
/**  Summary        : Handles Occurrence Code Data
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Class OccurrenceCode
    /// </summary>
    public class OccurrenceCode
    {
        /// <summary>
        /// Gets or sets the claim identifier.
        /// </summary>
        /// <value>
        /// The claim identifier.
        /// </value>
        public long ClaimId { set; get; }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public int Instance { set; get; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { set; get; }
    }
}
