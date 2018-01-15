/************************************************************************************************************/
/**  Author         : Prasad Dintakurty
/**  Created        : 05/Sep/2013
/**  Summary        : Handles Contract Model List
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the Node Id.
        /// </summary>
        /// <value>
        /// The Node Id.
        /// </value>
        public long NodeId { get; set; }

        /// <summary>
        /// Gets or sets the Node Text.
        /// </summary>
        /// <value>
        /// The Node Text.
        /// </value>
        public string NodeText { get; set; }
    }
}
