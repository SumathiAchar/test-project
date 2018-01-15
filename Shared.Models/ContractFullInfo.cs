/************************************************************************************************************/
/**  Author         :Raj Zalavadia
/**  Created        :09-Aug-2013
/**  Summary        :Handles Contract Full info
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractFullInfo:BaseModel
    {

        /// <summary>
        /// Set or Get Contract Basic Info.
        /// </summary>
        /// <value>
        /// The Contract Basic Info.
        /// </value>
        public Contract ContractBasicInfo { get; set; }

        /// <summary>
        /// Set or Get Contract Contact Ids.
        /// </summary>
        /// <value>
        /// The Contract Contact Ids.
        /// </value>
        public List<long> ContractContactIds { get; set; }

        /// <summary>
        /// Set or Get Contract Notes.
        /// </summary>
        /// <value>
        /// The Contract Notes.
        /// </value>
        public List<ContractNote> ContractNotes { get; set; }

        /// <summary>
        /// Set or Get Contract Docs.
        /// </summary>
        /// <value>
        /// The Contract Docs.
        /// </value>
        public List<ContractDoc> ContractDocs { get; set; }
    }
}
