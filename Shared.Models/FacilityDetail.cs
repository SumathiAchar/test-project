/************************************************************************************************************/
/**  Author         : Vishesh Bhaswar
/**  Created        : 02-Sep-2013
/**  Summary        : Handles Facility Details
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    public class FacilityDetail:BaseModel
    {
        /// <summary>
        /// Gets or sets the name of the facility.
        /// </summary>
        /// <value>
        /// The name of the facility.
        /// </value>
        public string FacilityName { get; set; }

        /// <summary>
        /// Gets or sets the node unique identifier.
        /// </summary>
        /// <value>
        /// The node unique identifier.
        /// </value>
        public long NodeId { get; set; }

        /// <summary>
        /// Gets or sets the user facility.
        /// </summary>
        /// <value>
        /// The user facility.
        /// </value>
        public string UserFacility { get; set; }
    }
}
